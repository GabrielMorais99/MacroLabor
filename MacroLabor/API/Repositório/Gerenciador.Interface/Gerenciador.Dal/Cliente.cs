using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gerenciador.Dal
{
    public class Cliente
    {
        public static Guid Salvar(Entidades.Cliente entidade)
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {
                    if (entidade.IdCliente == Guid.Empty)
                    {
                        entidade.IdCliente = Guid.NewGuid();

                        contexto.Entry(new TCliente()
                        {

                            Ativo = true,

                            Cliente = entidade.Nome,

                            IDCliente = entidade.IdCliente,

                            DataCriacao = DateTime.Now

                        }).State = System.Data.Entity.EntityState.Added;

                        contexto.SaveChanges();
                    }
                    else
                    {
                        var cliente = contexto.TCliente.First(x => x.IDCliente == entidade.IdCliente);

                        cliente.Ativo = entidade.Ativo;

                        cliente.Cliente = entidade.Nome;

                        contexto.SaveChanges();

                    }

                }

                return entidade.IdCliente;
            }
            catch (Exception)
            {
                throw new Exception("Falha na operação!");
            }

           

        }

        public static void Excluir(Guid idCliente)
        {
            using (var contexto = new GerenciadorContexto())
            {

                contexto.TCliente.First(x => x.IDCliente == idCliente).Ativo = false;

                contexto.SaveChanges();
            }
        }

        public static IEnumerable<Entidades.Cliente> Retornar_Clientes()
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                    return (from c in contexto.TCliente.Where(x => x.Ativo)



                            select new Entidades.Cliente()
                            {
                                Ativo = c.Ativo,

                                IdCliente = c.IDCliente,

                                Nome = c.Cliente,

                                DataCadastro = c.DataCriacao


                            }

                          ).OrderBy(x => x.Nome).ToArray();
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

                    return (from sc in contexto.TSistemaCliente

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

    }
}
