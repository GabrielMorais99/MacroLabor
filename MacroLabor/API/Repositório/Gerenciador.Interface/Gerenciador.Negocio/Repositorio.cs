using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Negocio
{
    public class Repositorio
    {
        public static Guid Salvar(RepositorioBase entidade)
        {
            IEnumerable<RepositorioBase> repositorios = Retornar_Repositorios();

            if (repositorios.Any(x => x.Nome == entidade.Nome && x.Servidor == entidade.Servidor && x.Usuario == entidade.Usuario  && x.IdRepositorio != entidade.IdRepositorio))
                throw new Exception("Repositório já cadastrado!");

            var rptCadastrados = repositorios.Where(x => x.Ativo && x.IdRepositorio != entidade.IdRepositorio).SelectMany(x => x.RepositoriosSistemaCliente.Where(p => p.Ativo));

            //Cadastro de um repositório vigente
            if (!entidade.Historica)
            {
                if (entidade.ApenasLeitura)
                    throw new Exception("Não é possível cadastrar um repositório vigente com permissão de apenas leitura!"); 

                foreach (var sc in entidade.SistemasClientes.Where(x=>x.Ativo))
                {
                    if (rptCadastrados.Any(x => !x.Historica && x.IdSistemaCliente == sc.IdSistemaCliente))
                    {
                        var auxiliar = rptCadastrados.First(x => !x.Historica && x.IdSistemaCliente == sc.IdSistemaCliente);

                        throw new Exception(string.Format("Não é possível cadastrar outro repositório vigente para {0} !", auxiliar.Nome));
                    }
                }

            }
            else
            { //Cadastro de um repositório histórico
                
                if(!entidade.ApenasLeitura)
                {

                    foreach (var sc in entidade.SistemasClientes.Where(x => x.Ativo))
                    {
                        if (rptCadastrados.Any(x => x.Historica && !x.ApenasLeitura && x.IdSistemaCliente == sc.IdSistemaCliente))
                        {
                            var auxiliar = rptCadastrados.First(x => !x.Historica && !x.ApenasLeitura && x.IdSistemaCliente == sc.IdSistemaCliente);

                            throw new Exception(string.Format("Não é possível cadastrar outro repositório histórico ativo para {0} !", auxiliar.Nome));
                        }
                    }
                }
            }

            return  Dal.Repositorio.Salvar(entidade);
        }

        public static IEnumerable<RepositorioBase> Retornar_Repositorios()
        {

            var lista = Dal.Repositorio.Retornar_Repositorios().Where(x=>x.Ativo);

            return lista;
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes(Guid idRepositorioClienteSistema)
        {
            var lista = Dal.Repositorio.Retornar_Sistemas_Clientes(idRepositorioClienteSistema);

            return lista;
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes()
        {
            var lista = Dal.Repositorio.Retornar_Sistemas_Clientes();

            return lista;
        } 

        public static RepositorioBase Retornar_Repositorio(Guid idRepositorio)
        {
            var lista =  Dal.Repositorio.Retornar_Repositorio(idRepositorio);

            return lista;
        }

        public static IEnumerable<Transferencia> Retornar_Transferencias(DateTime dataInicio, DateTime dataTermino)
        {
            var lista = Dal.Repositorio.Retornar_Transferencias(dataInicio,dataTermino);

            return lista;
        }

        public static void Excluir(Guid idRepositorio)
        {


             Dal.Repositorio.Excluir(idRepositorio);

        }

        public static bool Transferir(Guid[] idRepositorios)
        {
           return Dal.Gerenciador.Transferir(idRepositorios);
        }
    }
}
