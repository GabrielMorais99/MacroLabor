using Repositorio.Dal;
using System;
using System.Linq;

namespace Gerenciador.Dal
{
    public class Arquivo
    {
        public static GerenciadorRepositorio Gerenciador { get; set; }

        public Arquivo(Guid sistemaCliente) {

            if (Gerenciador != null)
            {
                if (Gerenciador.IdSistemaCliente != sistemaCliente)
                    Gerenciador = new GerenciadorRepositorio(sistemaCliente);
            }
            else
                Gerenciador = new GerenciadorRepositorio(sistemaCliente);

        }

        public Guid Gravar(string nomeArquivo, byte[] arquivo)
        {
            try
            {
               var idArquivo = new ArquivoDal(Gerenciador.RepositorioVigente.StringConexao).Gravar(nomeArquivo, arquivo);


               return Gerenciador.SalvarIndice(new IndiceDto() {

                   IdArquivo = idArquivo,

                   IdRepositorioSistemaCliente = Gerenciador.RepositorioVigente.IdRepositorioClienteSistema

               });
         
            }
            catch (Exception e)
            {
                throw;
            }
        }

  
     
        public ArquivoDto Obter_Arquivo(Guid indice)
        {
            try
            {
                var infor = Gerenciador.Indices.First(x => x.IdIndice == indice);

                var repositorio = Gerenciador.Repositorios.FirstOrDefault(x => x.IdRepositorioClienteSistema == infor.IdRepositorioSistemaCliente);

                return new ArquivoDal(repositorio.StringConexao).Obter_Arquivo(infor.IdArquivo);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
