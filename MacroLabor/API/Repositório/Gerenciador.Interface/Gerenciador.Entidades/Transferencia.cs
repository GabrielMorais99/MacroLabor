using System;
using System.ComponentModel.DataAnnotations;

namespace Gerenciador.Entidades
{
    public class Transferencia
    {

        [Display(Name = "Origem")]
        public string RepositorioOrigem { get; set; }


        [Display(Name = "Destino")]
        public string RepositorioDestino { get; set; }


        [Display(Name = "Data/Hora Início")]
        public DateTime DataInicioProcessamento { get; set; }


        [Display(Name = "Data/Hora Término")]
        public DateTime? DataTerminoProcessamento { get; set; }

        public string Cliente { get; set; }

        public string Sistema { get; set; }

        public bool? TransferenciaOK { get; set; }

        public string Status { get { return TransferenciaOK == null ? "N/A" : (TransferenciaOK.Value) ? "OK" : "NOK"; } }

        [Display(Name = "Arq. Transf.")]
        public int? TotalArquivosTransferidos { get; set; }

        public string Error { get; set; }



    }
}
