using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Gerenciador.Dal
{
    public class Sistema
    {
        public static IEnumerable<Entidades.Sistema> Retornar_Sistemas()
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                    var lista =  (from s in contexto.TSistema.Where(x => x.Ativo)



                            select new Entidades.Sistema()
                            {
                                Ativo = s.Ativo,

                                IdSistema = s.IDSistema,

                                Nome = s.Sistema,

                                DataCadastro = s.DataCriacao,

                                SistemasClientes = (from sc in contexto.TSistemaCliente.Where(x=>x.IDSistema == s.IDSistema)

                                                    join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                                                    select new SistemaCliente {

                                                       IdCliente = c.IDCliente,

                                                       Cliente = c.Cliente,

                                                       Ativo = sc.Ativo,

                                                       DataCadastro = sc.DataCriacao,

                                                       IdSistema = s.IDSistema,

                                                       IdSistemaCliente = sc.IDSistemaCliente,

                                                       Nome = sc.Nome,

                                                       Sistema = s.Sistema

                                                        
                                                    })


                            }

                          ).OrderBy(x => x.Nome).ToArray();

                    return lista;
                }
            }
            catch (Exception e)
            {

                throw;
            }



        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes(Guid idSistema)
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                    return (from sc in contexto.TSistemaCliente.Where(x=>x.IDSistema == idSistema)

                            join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                            join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                            select new SistemaCliente()
                            {

                                IdSistemaCliente = sc.IDSistemaCliente,

                                Nome = sc.Nome,

                                IdSistema = s.IDSistema,

                                IdCliente = c.IDCliente,

                                Sistema = s.Sistema,

                                Cliente = c.Cliente,

                                Ativo = sc.Ativo

                            }

                          ).ToArray();
                }
            }
            catch (Exception e)
            {

                throw;
            }



        }

        public static Guid Salvar(Entidades.Sistema entidade)
        {

            try
            {
                using (var transacao = new TransactionScope())
                {

                    using (var contexto = new GerenciadorContexto())
                    {
                        if (entidade.IdSistema == Guid.Empty)
                        {
                            entidade.IdSistema = Guid.NewGuid();

                            entidade.Ativo = true;

                            contexto.Entry(new TSistema()
                            {

                                Ativo = entidade.Ativo,

                                Sistema = entidade.Nome,

                                IDSistema = entidade.IdSistema,

                                DataCriacao = DateTime.Now

                            }).State = System.Data.Entity.EntityState.Added;



                            contexto.SaveChanges();
                        }
                        else
                        {
                            var registro = contexto.TSistema.First(x => x.IDSistema == entidade.IdSistema);

                            registro.Sistema = entidade.Nome;

                            contexto.SaveChanges();

                        }


                        #region  Vinculação entre sistemas e clientes... 

                        foreach (SistemaCliente item in entidade.SistemasClientes)
                        {

                            if (contexto.TSistemaCliente.Any(x => x.IDSistemaCliente == item.IdSistemaCliente))
                            {
                                var sistemaCliente = contexto.TSistemaCliente.First(x => x.IDSistemaCliente == item.IdSistemaCliente);

                                sistemaCliente.Nome = item.Nome == null ? string.Empty : item.Nome;

                                sistemaCliente.Ativo = item.Ativo;
                            }
                            else
                            {

                                contexto.Entry(new TSistemaCliente()
                                {

                                    Ativo = item.Ativo,

                                    DataCriacao = DateTime.Now,

                                    IDCliente = item.IdCliente,

                                    IDSistema = entidade.IdSistema,

                                    IDSistemaCliente = Guid.NewGuid(),

                                    Nome = item.Nome == null ? string.Empty : item.Nome

                                }).State = System.Data.Entity.EntityState.Added;

                            }

                        }

                        contexto.SaveChanges();
                        #endregion



                    }

                    transacao.Complete();
                }

                return entidade.IdSistema;
            }
            catch (Exception e)
            {
                throw new Exception("Falha na operação!");
            }



        }

        public static void Excluir(Guid idSistema)
        {
            using (var contexto = new GerenciadorContexto())
            {

                contexto.TSistema.First(x => x.IDSistema == idSistema).Ativo = false;

                contexto.SaveChanges();
            }
        } 
       

    }
}
