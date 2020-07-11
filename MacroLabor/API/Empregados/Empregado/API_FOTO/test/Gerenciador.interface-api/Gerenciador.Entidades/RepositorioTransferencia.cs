using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gerenciador.Entidades
{
    public  class RepositorioTransferencia
    {
        public RepositorioTransferencia()
        {
            IdHistoricoTransferencia = Guid.NewGuid();

            Blocos_Arquivos_Infor = new List<Bloco_Transferencia_Arquivo>();

            Indices = new List<Indice>();

            DataHoraInicio = DateTime.Now;
        }

        public Repositorio Repositorio_Origem { get; set; }

        public Repositorio Repositorio_Destino { get; set; }

        public IEnumerable<Indice> Indices { get; set; }

        public Guid IdHistoricoTransferencia { get; }

        [Display(Name = "Data/Hora Início")]
        public DateTime DataHoraInicio  { get; }

        [Display(Name = "Data/Hora Término")]
        public DateTime? DataHoraTermino { get; set; }

        [Display(Name = "Qtde.Arquivos Transf.")]
        public IEnumerable<Arquivo> Arquivos { get { return Blocos_Arquivos_Infor.SelectMany(x => x.Arquivos); } }

        [Display(Name = "Qtde.Arquivos Transf.")]
        public int Total_Arquivos_Transferidos { get { return Arquivos.Count(); } }

        public List<Bloco_Transferencia_Arquivo> Blocos_Arquivos_Infor { get; set; }

        public bool TranferenciaOK { get; set; }

        [Display(Name = "Status")]
        public string TranferenciaOK_ { get { return (TranferenciaOK) ? "Sucesso" : "Falha"; } }

        public Exception Exception { get; set; }
    }
}
