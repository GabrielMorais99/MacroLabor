using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Gerenciador.Dal
{

    /// <summary>
    /// 
    /// </summary>
    public  class Repositorio
    {  
        public static  Guid Salvar(RepositorioBase entidade)
        {

            using (var transacao = new TransactionScope())
            {

                using (var contexto = new GerenciadorContexto())
                {
                    if (entidade.IdRepositorio == Guid.Empty) Incluir(entidade, contexto); else Alterar(entidade, contexto);

                } 

                transacao.Complete();

            } 

            return entidade.IdRepositorio;
        }

        private static void Incluir(RepositorioBase entidade, GerenciadorContexto contexto)
        {
            entidade.IdRepositorio = Guid.NewGuid();

            contexto.Entry(new TRepositorio()
            {

                Nome = entidade.Nome,

                Senha = entidade.Senha,

                Servidor = entidade.Servidor,

                Usuario = entidade.Usuario,

                ApenasLeitura = entidade.ApenasLeitura,

                Ativo = true,

                DadosConexao = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", entidade.Servidor, entidade.Nome, entidade.Usuario, entidade.Senha),

                DataCriacao = DateTime.Now,

                Historica = entidade.Historica,

                IDRepositorio = entidade.IdRepositorio
            }).State = System.Data.Entity.EntityState.Added;

            contexto.SaveChanges();

            foreach (var item in entidade.SistemasClientes)
            {
                if (item.Ativo)
                {
                    contexto.Entry(new TRepositSistemaCliente()
                    {

                        Ativo = item.Ativo,

                        DataCriacao = DateTime.Now,

                        IDRepositorio = entidade.IdRepositorio, 

                        IDRepositSistemaCliente = Guid.NewGuid(),

                        IDSistemaCliente = item.IdSistemaCliente,

                        PrazoRetencaoDias = item.PrazoRetencao,

                    }).State = System.Data.Entity.EntityState.Added;
                }

                contexto.SaveChanges();

            }
        }

        private static void Alterar(RepositorioBase entidade, GerenciadorContexto contexto)
        {
            var repositorio = contexto.TRepositorio.First(x => x.IDRepositorio == entidade.IdRepositorio);

            repositorio.Nome = entidade.Nome;

            repositorio.Senha = entidade.Senha;

            repositorio.Servidor = entidade.Servidor;

            repositorio.Usuario = entidade.Usuario;

            repositorio.Historica = entidade.Historica;

            repositorio.ApenasLeitura = entidade.ApenasLeitura;

            repositorio.DadosConexao = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", entidade.Servidor, entidade.Nome, entidade.Usuario, entidade.Senha);

            contexto.SaveChanges();

            foreach (var item in entidade.SistemasClientes)
            {

                if (contexto.TRepositSistemaCliente.Any(x => x.IDRepositorio == entidade.IdRepositorio && x.IDSistemaCliente == item.IdSistemaCliente))
                {
                    contexto.TRepositSistemaCliente.First(x => x.IDRepositorio == entidade.IdRepositorio && x.IDSistemaCliente == item.IdSistemaCliente).Ativo = item.Ativo;

                    contexto.TRepositSistemaCliente.First(x => x.IDRepositorio == entidade.IdRepositorio && x.IDSistemaCliente == item.IdSistemaCliente).PrazoRetencaoDias = item.PrazoRetencao;
                }
                else
                {
                    if (item.Ativo)
                    {
                        contexto.Entry(new TRepositSistemaCliente()
                        {
                            Ativo = item.Ativo,

                            DataCriacao = DateTime.Now,

                            IDRepositorio = entidade.IdRepositorio,

                            IDRepositSistemaCliente = Guid.NewGuid(),

                            PrazoRetencaoDias = item.PrazoRetencao,

                            IDSistemaCliente = item.IdSistemaCliente

                        }).State = System.Data.Entity.EntityState.Added;
                    }


                }

                contexto.SaveChanges();
            }
        }

        public static RepositorioBase Retornar_Repositorio(Guid idRepositorio)
        {
            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                    return (from r in contexto.TRepositorio.Where(x => x.IDRepositorio == idRepositorio)


                            select new Entidades.RepositorioBase()
                            {
                                ApenasLeitura = r.ApenasLeitura,

                                Historica = r.Historica,

                                IdRepositorio = r.IDRepositorio,

                                StringConexao = r.DadosConexao,

                                Ativo = r.Ativo,

                                Nome = r.Nome,

                                Usuario = r.Usuario,

                                Senha = r.Senha,

                                Servidor = r.Servidor,

                                RepositoriosSistemaCliente = (from rsc in contexto.TRepositSistemaCliente.Where(x => x.IDRepositorio == r.IDRepositorio && x.Ativo)

                                                              join sc in contexto.TSistemaCliente on rsc.IDSistemaCliente equals sc.IDSistemaCliente

                                                              join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                                                              join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                                                              select new Entidades.Repositorio()
                                                              {

                                                                  Nome = sc.Nome,

                                                                  Cliente = c.Cliente,

                                                                  Sistema = s.Sistema,

                                                                  StringConexao = r.DadosConexao,

                                                                  ApenasLeitura = r.ApenasLeitura,

                                                                  Historica = r.Historica,

                                                                  IdRepositorio = r.IDRepositorio,

                                                                  Ativo = rsc.Ativo,

                                                                  IdRepositorioClienteSistema = rsc.IDRepositSistemaCliente,

                                                                  IdSistemaCliente = sc.IDSistemaCliente,

                                                                  IdSistema = s.IDSistema,

                                                                  IdCliente = c.IDCliente,

                                                                  PrazoRetencao = rsc.PrazoRetencaoDias,

                                                                  Conexao_Historico = contexto.TRepositSistemaCliente.Any(x => x.IDSistemaCliente == rsc.IDSistemaCliente &&

                                                                  !rsc.TRepositorio.Historica

                                                                  && x.Ativo && x.TRepositorio.Historica && x.TRepositorio.Ativo && !x.TRepositorio.ApenasLeitura) ? contexto.TRepositSistemaCliente

                                                                 .FirstOrDefault(x => x.IDSistemaCliente == rsc.IDSistemaCliente

                                                                  && x.Ativo && x.TRepositorio.Historica && x.TRepositorio.Ativo && !x.TRepositorio.ApenasLeitura).TRepositorio.DadosConexao : null,
                                                                  
                                                                //  LimiteMin = contexto.TIndice.Any(x => x.IDRepositorioSistemaCliente == rsc.IDRepositSistemaCliente)?
                                                                  
                                                                //  (DateTime?)contexto.TIndice.Where(x => x.IDRepositorioSistemaCliente == rsc.IDRepositSistemaCliente).Min(x=> x.DataCriacao):null,

                                                                 // LimiteMax = contexto.TIndice.Any(x => x.IDRepositorioSistemaCliente == rsc.IDRepositSistemaCliente) ?

                                                                //  (DateTime?)contexto.TIndice.Where(x => x.IDRepositorioSistemaCliente == rsc.IDRepositSistemaCliente).Max(x => x.DataCriacao) : null
                                                              })

                            }

                          ).FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static IEnumerable<RepositorioBase> Retornar_Repositorios()
        {
            try
            {
                using (var contexto = new GerenciadorContexto())
                {   

                    var lista =  (from r in contexto.TRepositorio


                            select new RepositorioBase()
                            {

                                ApenasLeitura = r.ApenasLeitura,

                                Historica = r.Historica,

                                IdRepositorio = r.IDRepositorio,

                                StringConexao = r.DadosConexao,

                                Nome = r.Nome,

                                Usuario = r.Usuario,

                                Senha = r.Senha,

                                Servidor = r.Servidor,

                                Ativo = r.Ativo, 

                                SistemasClientes = (from sc in contexto.TSistemaCliente.Where(x=>x.Ativo)

                                                    join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                                                    join s in contexto.TSistema on sc.IDSistema equals s.IDSistema 

                                                    select new SistemaCliente {

                                                        Ativo = contexto.TRepositSistemaCliente.Any(x=>x.Ativo && x.IDSistemaCliente == sc.IDSistemaCliente && x.IDRepositorio == r.IDRepositorio),

                                                        PrazoRetencao = contexto.TRepositSistemaCliente.Any(x => x.Ativo && x.IDSistemaCliente == sc.IDSistemaCliente && x.IDRepositorio == r.IDRepositorio) ?
                                                        
                                                        contexto.TRepositSistemaCliente.FirstOrDefault(x => x.Ativo && x.IDSistemaCliente == sc.IDSistemaCliente && x.IDRepositorio == r.IDRepositorio).PrazoRetencaoDias : null,

                                                        Cliente = c.Cliente,

                                                        Sistema = s.Sistema,

                                                        IdSistema = sc.IDSistema,

                                                        IdCliente = sc.IDCliente, 

                                                        IdSistemaCliente = sc.IDSistemaCliente,

                                                        Nome = sc.Nome

                                                    }),


                                RepositoriosSistemaCliente = (from rsc in contexto.TRepositSistemaCliente.Where(x => x.Ativo && x.IDRepositorio == r.IDRepositorio)

                                                              join sc in contexto.TSistemaCliente on rsc.IDSistemaCliente equals sc.IDSistemaCliente

                                                              join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                                                              join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                                                              select new Entidades.Repositorio()
                                                              {
                                                                  Nome = sc.Nome,

                                                                  Usuario = r.Usuario,

                                                                  Senha = r.Senha,

                                                                  Servidor = r.Servidor,

                                                                  Cliente = c.Cliente,

                                                                  Sistema = s.Sistema,

                                                                  StringConexao = r.DadosConexao,

                                                                  ApenasLeitura = r.ApenasLeitura,

                                                                  Historica = r.Historica,

                                                                  IdRepositorio = r.IDRepositorio,

                                                                  Ativo = rsc.Ativo,

                                                                  IdRepositorioClienteSistema = rsc.IDRepositSistemaCliente,

                                                                  IdSistemaCliente = sc.IDSistemaCliente,

                                                                  IdSistema = s.IDSistema,

                                                                  IdCliente = c.IDCliente,

                                                                  PrazoRetencao = rsc.PrazoRetencaoDias

                                                                  
                                                              })

                            }

                          ).OrderBy(x => x.Historica).ToArray();

                    return lista;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IEnumerable<Transferencia> Retornar_Transferencias(DateTime dataInicio, DateTime dataTermino)
        {
            try
            {
                using (var contexto = new GerenciadorContexto())
                {
                    dataTermino = dataTermino.AddDays(1).AddMilliseconds(-1);

                    var lista = (from r in contexto.THistoricoTransferencia.Where(x => x.DataHoraInicioTransferencia >= dataInicio && x.DataHoraInicioTransferencia <= dataTermino)

                                 join rsc_origem in contexto.TRepositSistemaCliente on r.IDRepositorioSistemaClienteOrigem equals rsc_origem.IDRepositSistemaCliente

                                 join r_origem in contexto.TRepositorio on rsc_origem.IDRepositorio equals r_origem.IDRepositorio

                                 join sc in contexto.TSistemaCliente on rsc_origem.IDSistemaCliente equals sc.IDSistemaCliente

                                 join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                                 join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                                 join rsc_dest in contexto.TRepositSistemaCliente on r.IDRepositorioSistemaClienteDestino equals rsc_dest.IDRepositSistemaCliente

                                 into temp

                                 from dest in temp.DefaultIfEmpty()

                                 join r_dest in contexto.TRepositorio on dest.IDRepositorio equals r_dest.IDRepositorio

                                 into temp2

                                 from dest2 in temp2.DefaultIfEmpty()


                                 select new Transferencia()
                                 {

                                     DataInicioProcessamento = r.DataHoraInicioTransferencia,

                                     DataTerminoProcessamento = r.DataHoraTerminoTransferencia,

                                     TransferenciaOK = r.TransferenciaOK,

                                     RepositorioOrigem = r_origem.Nome,

                                     RepositorioDestino = (dest2 == null) ? string.Empty : dest2.Nome,

                                     Sistema = s.Sistema,

                                     Cliente = c.Cliente,

                                     Error = r.Error,

                                     TotalArquivosTransferidos = r.QtdIndices


                                 }

                          ).OrderByDescending(x => x.DataInicioProcessamento).ThenBy(x => x.Cliente).ThenBy(x => x.Sistema).ToArray();

                    return lista;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }     

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes()
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                   
                    return (from sc in contexto.TSistemaCliente.Where(x => x.Ativo)

                            join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                            join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                            select new SistemaCliente()
                            {

                                IdSistemaCliente = sc.IDSistemaCliente,

                                Nome = sc.Nome,

                                Sistema = s.Sistema,

                                Cliente = c.Cliente

                            }

                          ).OrderBy(x => x.Nome).ToArray();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes(Guid idRepositorioClienteSistema)
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {


                    return (from sc in contexto.TSistemaCliente.Where(x => x.Ativo)

                            join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                            join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                            select new SistemaCliente()
                            {
                                Ativo = contexto.TRepositSistemaCliente.Any(x=> x.IDRepositSistemaCliente == idRepositorioClienteSistema),

                                PrazoRetencao = contexto.TRepositSistemaCliente.Any(x => x.IDRepositSistemaCliente == idRepositorioClienteSistema)? 
                                
                                contexto.TRepositSistemaCliente.FirstOrDefault(x => x.IDRepositSistemaCliente == idRepositorioClienteSistema).PrazoRetencaoDias :null,

                                IdSistemaCliente = sc.IDSistemaCliente,

                                Nome = sc.Nome,

                                Sistema = s.Sistema,

                                Cliente = c.Cliente

                                

                            }

                          ).OrderBy(x => x.Nome).ToArray();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static void Excluir(Guid idRepositorio)
        {
            using (var contexto = new GerenciadorContexto())
            {

                contexto.TRepositorio.First(x => x.IDRepositorio == idRepositorio).Ativo = false;

                contexto.SaveChanges();
            }
        }
    }
}
