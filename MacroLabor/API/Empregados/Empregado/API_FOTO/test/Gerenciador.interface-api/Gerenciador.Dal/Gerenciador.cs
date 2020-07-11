using Gerenciador.Entidades;
using Microsoft.SqlServer.Management.Smo;
using Repositorio.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Gerenciador.Dal
{
    public static class Gerenciador
    {

        #region Propriedades   

        private const int Tamanho_Bloco_Transferencia = 300;

        private static IEnumerable<RepositorioBase> repositoriosBase { get; set; }
        public static IEnumerable<RepositorioBase> RepositoriosBase {

            get
            {
                repositoriosBase = repositoriosBase ?? Repositorio.Retornar_Repositorios().Where(x=>x.Ativo);

                return repositoriosBase;
            }
        }      

        private static IEnumerable<Entidades.Repositorio> repositorios; 
        public static IEnumerable<Entidades.Repositorio> Repositorios
        {
            get
            {
                repositorios = repositorios ?? Retornar_Repositorios(); 

                return repositorios;
            }
            
        }

        private static IEnumerable<Entidades.Repositorio> Retornar_Repositorios() {

            return (RepositoriosBase.Any()) ? RepositoriosBase.SelectMany(x => x.RepositoriosSistemaCliente) : repositorios;
                
        }
        #endregion 

        #region Métodos para gravação e obtenção de arquivos nos repositórios
        public static Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Guid idSistemaCliente)
        {
            try
            {

                var destino = Repositorios.FirstOrDefault(x => !x.Historica && !x.ApenasLeitura && x.IdSistemaCliente == idSistemaCliente && x.Ativo);

                if (destino == null)
                    throw new Exception("Nenhum REPOSITÓRIO DESTINO ATIVO encontrado!");

                return Incluir(nomeArquivo, arquivo, destino);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Guid idRepositorio, Guid idSistemaCliente)
        {
            var destino = Repositorios.FirstOrDefault(x => x.IdSistemaCliente == idSistemaCliente && x.Ativo && x.IdRepositorio == idRepositorio);

            if (destino == null)
                throw new Exception("Nenhum REPOSITÓRIO DESTINO ATIVO encontrado!");

            return Incluir(nomeArquivo, arquivo, destino);
        }

        private static Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Entidades.Repositorio destino)
        {


            using (var transacao = new TransactionScope())
            {

                #region Salvando o arquivo no repositório...
                var entidade = ArquivoDal.Salvar(new Entidades.Arquivo()
                {
                    StringConexao = destino.StringConexao,

                    Conteudo = arquivo,

                    NomeArquivo = nomeArquivo,

                    Tamanho = arquivo.Length
                });
                #endregion

                try
                {
                    #region Salvando o indice no gerenciador...

                    entidade.Indice = Guid.NewGuid();
                    using (var contexto = new GerenciadorContexto())
                    {
                        contexto.Entry(new TIndice()
                        {

                            IDArquivo = entidade.IdArquivo,

                            NomeArquivo = nomeArquivo,

                            DataCriacao = DateTime.Now,

                            DataUltimaAlteracao = DateTime.Now,

                            IDIndice = entidade.Indice,

                            Ativo = true,

                            IDRepositorioSistemaCliente = destino.IdRepositorioClienteSistema

                        }).State = System.Data.Entity.EntityState.Added;

                        contexto.SaveChanges();
                    }
                    #endregion

                    entidade.Repositorio = destino.Nome;

                    transacao.Complete();
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.InnerException.Message);
                } 

                return entidade;
            }
        }

        public static Entidades.Arquivo Atualizar(string nomeArquivo, byte[] arquivo, Guid indice)
        {
            var entidade = Obter_Arquivo(indice);

            return Atualizar(

                new Entidades.Arquivo()
                {
                    NomeArquivo = nomeArquivo,

                    Ativo = entidade.Ativo,

                    Conteudo = arquivo,

                    Indice = indice,

                    IdArquivo = entidade.IdArquivo,

                    StringConexao = entidade.StringConexao,

                    IdRepositorioSistemaCliente = entidade.IdRepositorioSistemaCliente,

                    DataCadastro = entidade.DataCadastro,

                    Tamanho = entidade.Tamanho
                });
        }

        public static Entidades.Arquivo Excluir(Guid indice)
        {
            try
            {

                var entidade = Obter_Arquivo(indice);

                entidade.Ativo = false;

                return Atualizar(entidade);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private  static Entidades.Arquivo Atualizar(Entidades.Arquivo entidade)
        {
            using (var transacao = new TransactionScope())
            {

                var retorno = ArquivoDal.Salvar(entidade);

                try
                { // Atualizando o indice
                    using (var contexto = new GerenciadorContexto())
                    {
                        var indice = contexto.TIndice.First(x => x.IDIndice == entidade.Indice);

                        indice.NomeArquivo = entidade.NomeArquivo;

                        indice.DataUltimaAlteracao = DateTime.Now;

                        indice.Ativo = entidade.Ativo;

                        contexto.SaveChanges();
                    }

                    transacao.Complete();
                }
                catch (Exception e)
                {

                    throw new Exception(e.InnerException.InnerException.Message);
                }

                return retorno;

            }
        }
        
        public static IEnumerable<Entidades.Arquivo> Obter_Arquivos(Guid idRepositorio, Guid IdSistemaCliente, string filtro, int page, int take)
        {
           var repositorio = Repositorios.First(x => x.IdSistemaCliente == IdSistemaCliente && x.IdRepositorio == idRepositorio);

            var entidades = ArquivoDal.Obter_Arquivos(repositorio.StringConexao, filtro, page, take).Where(x=>x.Ativo);

            IEnumerable<Indice> indices = new List<Indice>();

            using (var contexto = new GerenciadorContexto())
            {
                var arquivos = entidades.Select(t => t.IdArquivo).ToArray();

                indices = (from i in contexto.TIndice.Where(x => x.Ativo && arquivos.Contains(x.IDArquivo))

                           select new Indice { IdIndice = i.IDIndice, IdArquivo = i.IDArquivo }).ToArray();
            }

            return Retornar_Arquivos(repositorio, entidades, indices);

        }        

        public static Entidades.Arquivo Obter_Arquivo(Guid indice)
        {
            try
            {
                Indice indice_;

                using (var contexto = new GerenciadorContexto())
                {
                    indice_ = (from i in contexto.TIndice.Where(x => x.IDIndice == indice)
                                                              

                               select new Indice
                               {                                  

                                   IdIndice = i.IDIndice,

                                   IdArquivo = i.IDArquivo,

                                   IdRepositorioSistemaCliente = i.IDRepositorioSistemaCliente

                               }).FirstOrDefault();
                }


                if (indice_ == null)
                    throw new Exception(string.Format("Não foi possível realizar a busca deste arquivo pois a referência (Índice:{0}) para a pesquisa não foi encontrada!",indice.ToString()));

                var repositorio = Repositorios.FirstOrDefault(x => x.IdRepositorioClienteSistema == indice_.IdRepositorioSistemaCliente);

                var entidade = ArquivoDal.Obter_Arquivo(repositorio.StringConexao, indice_.IdArquivo);

                if (entidade == null)
                    throw new Exception("Arquivo não encontrado no repositório!");

                var lista =  Retornar_Arquivos(repositorio, new ArquivoDto[] { entidade }, new Indice[] { indice_ }).First() ;

                return lista;
              
            }
            catch (Exception)
            {
                throw;
            }
        } 

        private static IEnumerable<Entidades.Arquivo> Retornar_Arquivos(Entidades.Repositorio repositorio, IEnumerable<ArquivoDto> entidades, IEnumerable<Indice> indices)
        {
            List<Entidades.Arquivo> lista = new List<Entidades.Arquivo>();

            foreach (var entidade in entidades)
            {
                var i = indices.FirstOrDefault(x => x.IdArquivo == entidade.IdArquivo);

                lista.Add(new Entidades.Arquivo()
                {

                    Indice = (i == null) ? Guid.Empty : i.IdIndice,

                    Ativo = entidade.Ativo,

                    Sistema = repositorio.Sistema,

                    Cliente = repositorio.Cliente,

                    DataCadastro = entidade.DataCadastro,

                    IdArquivo = entidade.IdArquivo,

                    NomeArquivo = entidade.NomeArquivo,

                    StringConexao = repositorio.StringConexao,

                    Tamanho = entidade.Tamanho,

                    Historico = repositorio.Historica,

                    IdRepositorio = repositorio.IdRepositorio,

                    IdRepositorioSistemaCliente = repositorio.IdRepositorioClienteSistema,

                    IdSistemaCliente = repositorio.IdSistemaCliente,

                    Repositorio = repositorio.Nome,

                    Conteudo = entidade.Arquivo

                });
            }

            return lista;
        }

        #endregion


        #region Metodos Auxiliares
        public static bool Transferir(IEnumerable<Guid> repositorios)
        {
            bool TransferenciasOK = true;

            foreach (Entidades.Repositorio rptOrigem in Repositorios.Where(x => repositorios.Contains(x.IdRepositorio)))
            {
                TransferenciasOK = (TransferenciasOK && Transferir(rptOrigem));
            }

            return TransferenciasOK;
        }
        
        private static bool Transferir(Entidades.Repositorio rptOrigem)
        {
            var idHistoricoTransferencia = Guid.NewGuid();

            try
            {
                using (var contexto = new GerenciadorContexto())
                {
                    #region Iniciando a transferência de um repositório cliente - sistema 

                    contexto.Entry(new THistoricoTransferencia()
                    {
                        IDHistoricoTransferencia = idHistoricoTransferencia,

                        DataHoraInicioTransferencia = DateTime.Now,

                        IDRepositorioSistemaClienteOrigem = rptOrigem.IdRepositorioClienteSistema

                    }).State = System.Data.Entity.EntityState.Added;

                    contexto.SaveChanges();

                    #endregion 

                    var historicoTransferencia = contexto.THistoricoTransferencia.First(x => x.IDHistoricoTransferencia == idHistoricoTransferencia);

                    if (EhPermitido_Transferir(rptOrigem, historicoTransferencia)) {


                        #region Recuperando o repositório destino e atualizando o histórico no banco de dados...
                        var rptDestino = Repositorios.First(x => x.Ativo && x.IdSistemaCliente == rptOrigem.IdSistemaCliente && x.Historica && !x.ApenasLeitura);

                        historicoTransferencia.IDRepositorioSistemaClienteDestino = rptDestino.IdRepositorioClienteSistema;

                        var dias = Convert.ToInt32(rptOrigem.PrazoRetencao.Value);

                        var dataLimite = DateTime.Now.Date.AddDays(dias * -1);

                        #endregion

                        #region Recuperando os índices dos arquivos a serem tranferidos...

                        var indices = Obter_Indices(rptOrigem.IdRepositorioClienteSistema, dataLimite);

                        historicoTransferencia.QtdIndices = indices.Count();

                        contexto.SaveChanges();
                        #endregion 

                        if (indices.Any()) {

                            for (int i = 0; i < indices.Count(); i = i + Tamanho_Bloco_Transferencia)
                            {
                                var idBlocoTransferencia = Guid.NewGuid();

                                try
                                {
                                    #region Iniciando a transferência por bloco...
                                    contexto.Entry(new TBlocoTransferencia
                                    {
                                        IdBlocoTranferencia = idBlocoTransferencia,

                                        DataHoraInicioTransferencia = DateTime.Now,

                                        IdHistoricoTransferencia = historicoTransferencia.IDHistoricoTransferencia

                                    }).State = System.Data.Entity.EntityState.Added;

                                    contexto.SaveChanges();
                                    #endregion



                                    #region TRANSFERINDO!!!!!

                                    var blocoTransferencia = contexto.TBlocoTransferencia.First(x => x.IdBlocoTranferencia == idBlocoTransferencia);

                                    TransactionOptions transactionOptions = new TransactionOptions();

                                    transactionOptions.IsolationLevel = IsolationLevel.ReadCommitted;

                                    transactionOptions.Timeout = TimeSpan.MaxValue;

                                    using (var transacao = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                                    {

                                        //Realizando a transferência do bloco de arquivos!!
                                        var arquivos = ArquivoDal.Transferir(rptOrigem.StringConexao, rptDestino.StringConexao,

                                        indices.Skip(i).Take(Tamanho_Bloco_Transferencia).Select(x => x.IdArquivo).ToArray());

                                        blocoTransferencia.TamanhoBytes = arquivos.Sum(x => x.Tamanho);

                                        //Atualizando os índices!!

                                        foreach (var arquivo in arquivos)
                                        {
                                            var indice = contexto.TIndice.First(x => x.IDArquivo == arquivo.IdArquivoOrigem);

                                            indice.IDArquivo = arquivo.IdArquivo;

                                            indice.IDRepositorioSistemaCliente = rptDestino.IdRepositorioClienteSistema;

                                            indice.DataUltimaAlteracao = DateTime.Now;
                                            
                                            contexto.SaveChanges();

                                            contexto.Entry(new TArquivoTransferencia()
                                            {

                                                IdArquivoTransferencia = Guid.NewGuid(),

                                                IdBlocoTransferencia = blocoTransferencia.IdBlocoTranferencia,

                                                IdIndice = indices.First(x => x.IdArquivo == arquivo.IdArquivoOrigem).IdIndice

                                            }).State = System.Data.Entity.EntityState.Added;

                                            contexto.SaveChanges();
                                        }

                                        transacao.Complete();

                                    }

                                    #endregion


                                    #region Finalizando a transferência do bloco.. 

                                    blocoTransferencia.TranferenciaOK = true;

                                    blocoTransferencia.DataHoraTerminoTransferencia = DateTime.Now;

                                    contexto.SaveChanges();
                                    #endregion 


                                }
                                catch (Exception e)
                                {
                                    var blocoTransferencia = contexto.TBlocoTransferencia.First(x => x.IdBlocoTranferencia == idBlocoTransferencia);

                                    blocoTransferencia.TranferenciaOK =   false;

                                    historicoTransferencia.TransferenciaOK = false;

                                    blocoTransferencia.DataHoraTerminoTransferencia = DateTime.Now;

                                    blocoTransferencia.Error = e.Message;

                                    contexto.SaveChanges();
                                }

                            }

                        }
                    }

                    #region Finalizando a transferência do repositório cliente - sistema (OK)

                    historicoTransferencia.TransferenciaOK = true;

                    historicoTransferencia.DataHoraTerminoTransferencia = DateTime.Now;

                    contexto.SaveChanges();

                    #endregion 


                    return (bool) historicoTransferencia.TransferenciaOK;
                }

                 
            }
            catch (Exception e)
            {
                using (var contexto = new GerenciadorContexto())
                {

                    #region Finalizando a transferência do repositório cliente - sistema (ERROR)

                    var historicoTransferencia = contexto.THistoricoTransferencia.First(x => x.IDHistoricoTransferencia == idHistoricoTransferencia);

                    historicoTransferencia.TransferenciaOK = false;

                    historicoTransferencia.DataHoraTerminoTransferencia = DateTime.Now;

                    historicoTransferencia.Error = e.Message;

                    contexto.SaveChanges();

                    #endregion 
                }
            }

            return false;
        }
        

        private static IEnumerable<Indice> Obter_Indices(Guid idRepositorioSistemaCliente, DateTime dataLimite)
        {
            using (var contexto = new GerenciadorContexto())
            {
                return (from i in contexto.TIndice.Where(x => x.IDRepositorioSistemaCliente == idRepositorioSistemaCliente && x.DataCriacao <= dataLimite)
                        
                        select new Indice {

                            DataCriacao = i.DataCriacao,

                            DataUltimaAtualizacao = i.DataUltimaAlteracao,

                            IdArquivo = i.IDArquivo,

                            IdIndice = i.IDIndice,

                            IdRepositorioSistemaCliente = i.IDRepositorioSistemaCliente,

                            NomeArquivo = i.NomeArquivo

                        }).ToArray();
            }

        }

        private static bool EhPermitido_Transferir(Entidades.Repositorio repositorio, THistoricoTransferencia transferencia)
        {
            if (repositorio.Historica)
            {
                transferencia.Error = string.Format("Transferência não iniciada para o repositório {0} pois esta operação somente é permitida para repositórios vigentes.", repositorio.Nome);

                transferencia.TransferenciaOK = false;
            }
            else
            {
                if (!Repositorios.Any(x => x.Ativo && x.IdSistemaCliente == repositorio.IdSistemaCliente && x.Historica && !x.ApenasLeitura))
                {
                    transferencia.Error = string.Format("Transferência não iniciada para o repositório {0} pois este não possui um repositório de destino.", repositorio.Nome);

                    transferencia.TransferenciaOK = false;
                }
                else {

                    if(Repositorios.Count(x => x.Ativo && x.IdSistemaCliente == repositorio.IdSistemaCliente && x.Historica && !x.ApenasLeitura)>1){
                        
                        transferencia.Error = string.Format("A transferência de arquivos somente poderá ser realizada para um repositório destino.", repositorio.Nome);

                        transferencia.TransferenciaOK = false;

                    }

                }
            }

            return (transferencia.TransferenciaOK == null);
        }

        public static void LimparCacheRepositorios()
        {
            repositorios = null;
            repositoriosBase  = null;
        }

        #endregion



    }
}
