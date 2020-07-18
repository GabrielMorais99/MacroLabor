using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Repositorio.Dal
{
    public class ArquivoDal
    {
        public RepositorioContexto Contexto { get; set; }
        public ArquivoDal(string connectionString)
        {

            Contexto = new RepositorioContexto();

            Contexto.Database.GetDbConnection().ConnectionString = connectionString;
        }

        private string StringConexao { get; set; }

        public Guid Gravar(string nomeArquivo, byte[] arquivo)
        {

            using (Contexto)
            {
                Guid chave = Guid.NewGuid();

                Contexto.Entry(new Arquivo()
                {
                    IdArquivo = chave,

                    IdRepositorio = 1,

                    Arquivo1 = arquivo,

                    Ativo = true,

                    TamanhoBytes = arquivo.Length,

                    Nome = nomeArquivo,

                    Registro = DateTime.Now,

                    Tipo = string.Empty

                }).State = EntityState.Added;

                Contexto.SaveChanges();

                return chave;
            }


        }

        public ArquivoDto Obter_Arquivo(Guid idArquivo)
        {
            using (var contexto = new RepositorioContexto())
            {
                return (from rsc in contexto.Arquivo.Where(x => x.IdArquivo == idArquivo)

                        select new ArquivoDto()
                        {
                            IdArquivo = rsc.IdArquivo,

                            NomeArquivo = rsc.Nome,

                            Arquivo = rsc.Arquivo1,

                            DataCadastro = rsc.Registro,

                            Tamanho = rsc.TamanhoBytes,

                            Ativo = rsc.Ativo


                        }).FirstOrDefault();

            }
        }

    }
}

