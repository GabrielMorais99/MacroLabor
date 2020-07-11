using System;
using System.Collections.Generic;

namespace Gerenciador.Entidades
{
    public class Bloco_Transferencia_Arquivo
    {
        public Bloco_Transferencia_Arquivo()
        {
            DataHoraInicio = DateTime.Now;

            Arquivos = new List<Arquivo>();

            IdBlocoTransferencia = Guid.NewGuid();
        }

        public Guid IdTransferencia { get; set; }

        public Guid IdBlocoTransferencia { get; }

        public DateTime DataHoraInicio { get;   }

        public DateTime? DataHoraTermino { get; set; }

        public IEnumerable<Indice> Indices { get; set; }

        public IEnumerable<Arquivo> Arquivos { get; set; }

        public Exception Exception { get; set; } 

        public bool TranferenciaOK { get; set; }

    }
}
