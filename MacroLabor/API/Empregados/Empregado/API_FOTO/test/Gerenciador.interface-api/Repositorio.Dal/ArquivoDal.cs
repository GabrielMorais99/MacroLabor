using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Repositorio.Dal
{
    public static class ArquivoDal
    { 
        public static Gerenciador.Entidades.Arquivo Salvar(Gerenciador.Entidades.Arquivo entidade)
        {

            try
            {
                using (var Contexto = new RepositorioContexto())
                {
                    Contexto.Database.Connection.ConnectionString = entidade.StringConexao;

                    if (entidade.IdArquivo == Guid.Empty)
                    {
                        entidade.IdArquivo = Guid.NewGuid();

                        entidade.DataCadastro = DateTime.Now;

                        Contexto.Entry(new TArquivo()
                        {
                            IdArquivo = entidade.IdArquivo,

                            Arquivo = entidade.Conteudo,

                            Ativo = true,

                            TamanhoBytes = entidade.Conteudo.Length,

                            Nome = entidade.NomeArquivo,

                            DataCadastro = entidade.DataCadastro

                        }).State = EntityState.Added;
                    }

                    else
                    {

                        var arquivo = Contexto.TArquivo.First(x => x.IdArquivo == entidade.IdArquivo);

                        arquivo.Arquivo = entidade.Conteudo;

                        arquivo.Ativo = entidade.Ativo;

                        arquivo.TamanhoBytes = entidade.Conteudo.Length;

                        arquivo.Nome = entidade.NomeArquivo;

                    }

                    Contexto.SaveChanges();

                    return entidade;
                }
            }
            catch (Exception)
            {

                throw;
            }                  
          
        }

        public static IEnumerable<ArquivoDto> Obter_Arquivos(string conexao,string filtro, int page, int take)
        {
            try
            {
                using (var Contexto = new RepositorioContexto())
                {
                    Contexto.Database.Connection.ConnectionString = conexao;

                    var skip = (page - 1) * take;

                    var condicao = (filtro == string.Empty) ? true : false;

                    return  (from rsc in Contexto.TArquivo.Where(x => x.Ativo && (filtro == string.Empty) ? true : x.Nome.Contains(filtro)).OrderBy(x => x.Nome).Take(take)

                                 select new ArquivoDto()
                                 {
                                     IdArquivo = rsc.IdArquivo,

                                     NomeArquivo = rsc.Nome,

                                     DataCadastro = rsc.DataCadastro,

                                     Tamanho = rsc.TamanhoBytes,

                                     Ativo = rsc.Ativo


                                 }).ToArray();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Não foi possível estabelecer uma conexão com o repositório. Verifique a existência deste repositório no seu banco de dados e tente novamente.");
            }
        }

        public static IEnumerable<ArquivoDto> Obter_Arquivos(string conexao, Guid[] idArquivos)
        {


            try
            {
                using (var Contexto = new RepositorioContexto())
                {
                    Contexto.Database.Connection.ConnectionString = conexao;

                    return (from rsc in Contexto.TArquivo.Where(x => idArquivos.Contains(x.IdArquivo))

                            select new ArquivoDto()
                            {
                                IdArquivo = rsc.IdArquivo,

                                NomeArquivo = rsc.Nome,

                                Arquivo = rsc.Arquivo,

                                DataCadastro = rsc.DataCadastro,

                                Tamanho = rsc.TamanhoBytes,

                                Ativo = rsc.Ativo


                            }).ToArray();

                }
            }
            catch (Exception)
            {
                throw new Exception("Não foi possível estabelecer uma conexão com o repositório. Verifique a existência deste repositório no seu banco de dados e tente novamente.");
            } 
        }

        public static ArquivoDto Obter_Arquivo(string conexao, Guid idArquivo)
        {

            try
            {
                using (var Contexto = new RepositorioContexto())
                {
                    Contexto.Database.Connection.ConnectionString = conexao;

                    return (from rsc in Contexto.TArquivo.Where(x => x.IdArquivo == idArquivo)

                            select new ArquivoDto()
                            {
                                IdArquivo = rsc.IdArquivo,

                                NomeArquivo = rsc.Nome,

                                Arquivo = rsc.Arquivo,

                                DataCadastro = rsc.DataCadastro,

                                Tamanho = rsc.TamanhoBytes,

                                Ativo = rsc.Ativo


                            }).FirstOrDefault();

                }
            }
            catch (Exception ex)
            {
                //StringBuilder msg = new StringBuilder();

                //msg.AppendLine("Não foi possível estabelecer uma conexão com o repositório.");

                //msg.AppendLine(string.Format("Conexão: {0}", conexao));

                //msg.AppendLine(string.Format("ID Arquivo: {0}", idArquivo.ToString()));

                //msg.AppendLine(string.Format("Exception Original: {0}", ex.Message.ToString()));

                throw new Exception(ex.Message);
            }


           
        }

        public static IEnumerable<ArquivoDto> Transferir(string conexaoOrigem, string conexaoDestino, Guid[] idArquivos)
        {
            IEnumerable<ArquivoDto> arquivos = Obter_Arquivos(conexaoOrigem, idArquivos);

            List<TArquivo> entidades = new List<TArquivo>();

            if (arquivos.Any())
            {

                #region Incluindo os arquivos no destino...
                using (var contexto = new RepositorioContexto())
                {
                    contexto.Database.Connection.ConnectionString = conexaoDestino; 

                    #region Convertendo para a entidade a ser persistida no banco de dados.
                    foreach (var arquivo in arquivos)
                    {
                        arquivo.IdArquivoOrigem = arquivo.IdArquivo;

                        arquivo.IdArquivo = Guid.NewGuid();

                        entidades.Add(new TArquivo()
                        {

                            IdArquivo =arquivo.IdArquivo,

                            Arquivo = arquivo.Arquivo,

                            Ativo = arquivo.Ativo,

                            DataCadastro = DateTime.Now,

                            Nome = arquivo.NomeArquivo,

                            TamanhoBytes = arquivo.Tamanho
                        });
                    }

                    #endregion

                    //Realizando a transferência!!
                    Utils.BulkInsert(conexaoDestino, "TArquivo", entidades);

                    contexto.SaveChanges();
                    
                }
                #endregion 

                #region Excluindo os arquivos da origem
                using (var contexto = new RepositorioContexto())
                {
                    contexto.Database.Connection.ConnectionString = conexaoOrigem; 

                    contexto.TArquivo.RemoveRange(contexto.TArquivo.Where(x => idArquivos.Contains(x.IdArquivo)));

                    contexto.SaveChanges();

                }
                #endregion
            }

            return arquivos;
        } 

    }
}
