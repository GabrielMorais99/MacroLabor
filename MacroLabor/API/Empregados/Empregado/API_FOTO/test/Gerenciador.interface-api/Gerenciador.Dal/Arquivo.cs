using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gerenciador.Dal
{
    public class Arquivo
    {
 
        public Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Guid idSistemaCliente)
        {
            return Gerenciador.Incluir(nomeArquivo, arquivo, idSistemaCliente);
        }

        public Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Guid idRepositorio, Guid idSistemaCliente)
        {
            return Gerenciador.Incluir(nomeArquivo, arquivo, idRepositorio, idSistemaCliente);
        }

        public Entidades.Arquivo Atualizar(string nomeArquivo, byte[] arquivo, Guid indice)
        {
            return  Gerenciador.Atualizar(nomeArquivo, arquivo, indice);
        }

        public void Excluir(Guid indice)
        {
              Gerenciador.Excluir(indice);
        }

        public Entidades.Arquivo Obter_Arquivo(Guid indice)
        {
            return Gerenciador.Obter_Arquivo(indice);
        }

        public IEnumerable<Entidades.Arquivo> Obter_Arquivos(Guid idRepositorio,Guid idSistemaCliente, string filtro, int page, int take)
        {
            return Gerenciador.Obter_Arquivos(idRepositorio,idSistemaCliente,filtro,page, take);
        }

        public static IEnumerable<RepositorioBase> Retornar_Repositorios()
        {
            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                    return (from r in contexto.TRepositorio.Where(x => contexto.TRepositSistemaCliente.Where(t => t.Ativo).Select(t => t.IDRepositorio)

                     .Contains(x.IDRepositorio)).OrderBy(x => x.Nome).OrderBy(x => x.Nome)


                            select new Entidades.RepositorioBase()
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

                                RepositoriosSistemaCliente = (from rsc in contexto.TRepositSistemaCliente.Where(x => x.Ativo && x.IDRepositorio == r.IDRepositorio)

                                                              join sc in contexto.TSistemaCliente on rsc.IDSistemaCliente equals sc.IDSistemaCliente

                                                              join s in contexto.TSistema on sc.IDSistema equals s.IDSistema

                                                              join c in contexto.TCliente on sc.IDCliente equals c.IDCliente

                                                              select new Entidades.Repositorio()
                                                              {
                                                                  Nome = r.Nome,

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
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes(Guid idRepositorio)
        {

            try
            {
                using (var contexto = new GerenciadorContexto())
                {

                    var idSistemasClientes = contexto.TRepositSistemaCliente.Where(x => x.IDRepositorio == idRepositorio && x.Ativo).Select(x => x.IDSistemaCliente);

                    return (from sc in contexto.TSistemaCliente.Where(x => x.Ativo && idSistemasClientes.Contains(x.IDSistemaCliente))

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

    }
}
