using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gerenciador.Negocio
{
    public  class Arquivo
    {
        public static Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Guid IdSistemaCliente)
        {
            return new Dal.Arquivo().Incluir(nomeArquivo, arquivo, IdSistemaCliente);
        }

        public static Entidades.Arquivo Incluir(string nomeArquivo, byte[] arquivo, Guid idRepositorio, Guid IdSistemaCliente)
        {
            return new Dal.Arquivo().Incluir(nomeArquivo, arquivo, idRepositorio, IdSistemaCliente);
        }

        public static Entidades.Arquivo Atualizar(string nomeArquivo, byte[] arquivo, Guid indice)
        {
            return new Dal.Arquivo().Atualizar(nomeArquivo, arquivo, indice);
        }

        public static Entidades.Arquivo Obter_Arquivo(Guid indice)
        {
            return new Dal.Arquivo().Obter_Arquivo(indice);
        }

        public static IEnumerable<Entidades.Arquivo> Obter_Arquivos(Guid idRepositorio,Guid idSistemaCliente,string filtro, int page, int take)
        {
            return new Dal.Arquivo().Obter_Arquivos(idRepositorio,idSistemaCliente,filtro,page,take);
        }

        public static void Excluir_Arquivo(Guid indice)
        {
            new Dal.Arquivo().Excluir(indice);
        }

        public static IEnumerable<RepositorioBase> Retornar_Repositorios()
        {

            var lista = Dal.Arquivo.Retornar_Repositorios().Where(x => x.Ativo);

            return lista;
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes(Guid idRepositorio)
        {
            var lista = Dal.Arquivo.Retornar_Sistemas_Clientes(idRepositorio);

            return lista;
        }

        public static IEnumerable<Entidades.Repositorio> Retornar_Repositorios_Cache()
        {
            return Dal.Gerenciador.Repositorios;

        }

        public static void LimparCacheRepositorios()
        {
            Dal.Gerenciador.LimparCacheRepositorios();
        }
    }
}
