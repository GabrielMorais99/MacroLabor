using System;
using System.Collections.Generic;
using System.Linq;

namespace Gerenciador.Dal
{

    /// <summary>
    /// Responsabilidade da classe: Controlar os repositórios criados para os clientes.
    /// </summary>
    public  class GerenciadorRepositorio
    {

        public GerenciadorRepositorio() { Repositorios = Obter_Repositorios(); Indices =  Obter_Indices(); }

        public GerenciadorRepositorio(Guid idSistemaCliente) {


            IdSistemaCliente = idSistemaCliente;

            Repositorios =  Obter_Repositorios().Where(x=>x.IdSistemaCliente == IdSistemaCliente);

            Indices =  Obter_Indices();
        }

        public RepositorioDto RepositorioVigente {

            get {

                return (Repositorios.FirstOrDefault(x => x.IdSistemaCliente == IdSistemaCliente && !x.Historica));
                
            } }

        public IEnumerable<RepositorioDto> RepositoriosHistorico
        {
            get
            {
                return (Repositorios.Where(x => x.IdSistemaCliente == IdSistemaCliente && x.Historica && !x.ApenasLeitura));

            }
        }

        public Guid? IdSistemaCliente { get; set; }

        public IEnumerable<RepositorioDto> Repositorios { get; set; }

        public IEnumerable<IndiceDto> Indices { get; set; }

        private IEnumerable<RepositorioDto> Obter_Repositorios()
        {
            using (var contexto = new GerenciadorContexto())
            {
                return  (from rsc in contexto.TRepositSistemaCliente.Where(x=>x.Ativo)

                                join r in contexto.TRepositorio

                                on rsc.IDRepositorio equals r.IDRepositorio

                                select new RepositorioDto() {

                                    IdRepositorioClienteSistema = rsc.IDRepositSistemaCliente,

                                    IdRepositorio = r.IDRepositorio,

                                    ApenasLeitura = r.ApenasLeitura,

                                    Historica = r.Historica,

                                    NomeBaseDados = r.NomeBaseDados,

                                    StringConexao = r.DadosConexao,

                                    IdSistemaCliente = rsc.IDSistemaCliente,

                                    PrazoRetencao = rsc.PrazoRetencaoDias

                                }).ToArray();

            }
        }

        private IEnumerable<IndiceDto> Obter_Indices()
        {
            using (var contexto = new GerenciadorContexto())
            {
                var ids = Repositorios.Select(t => t.IdRepositorioClienteSistema);

                return (from rsc in contexto.TIndice.Where(x=> ids.Contains(x.IDRepositorioSistemaCliente))  

                        select new IndiceDto()
                        {

                            IdIndice = rsc.IDIndice,

                            IdArquivo = rsc.IDArquivo, 

                            IdRepositorioSistemaCliente = rsc.IDRepositorioSistemaCliente 

                        }).ToArray();

            }
        }

        public  Guid SalvarIndice(IndiceDto entidade)
        {
            using (var contexto = new GerenciadorContexto())
            {
                if (entidade.IdIndice == Guid.Empty)
                {
                    entidade.IdIndice = Guid.NewGuid();

                    contexto.Entry(new TIndice() {

                        IDArquivo = entidade.IdArquivo,

                        DataCriacao = DateTime.Now,

                        IDIndice = entidade.IdIndice,

                        IDRepositorioSistemaCliente = entidade.IdRepositorioSistemaCliente

                    }).State = System.Data.Entity.EntityState.Added;

                    contexto.SaveChanges();
                }
                else
                {
                    contexto.Entry(new TIndice()
                    {

                        IDArquivo = entidade.IdArquivo,

                        DataCriacao = DateTime.Now,

                        IDIndice = entidade.IdIndice,

                        IDRepositorioSistemaCliente = entidade.IdRepositorioSistemaCliente

                    }).State = System.Data.Entity.EntityState.Modified;

                    contexto.SaveChanges();
                }

                return entidade.IdIndice;
            }
        }




    }
}
