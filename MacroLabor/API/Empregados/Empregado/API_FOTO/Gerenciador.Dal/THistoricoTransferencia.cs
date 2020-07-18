using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class THistoricoTransferencia
    {
        public System.Guid IDHistoricoTransferencia { get; set; }
        public System.Guid IDRepositorioSistemaClienteOrigem { get; set; }
        public System.Guid IDRepositorioSistemaClienteDestino { get; set; }
        public System.DateTime DataHoraInicioTransferencia { get; set; }
        public Nullable<System.DateTime> DataHoraTerminoTransferencia { get; set; }
        public Nullable<bool> TransferenciaOK { get; set; }

        public virtual TRepositSistemaCliente TRepositSistemaCliente { get; set; }
        public virtual TRepositSistemaCliente TRepositSistemaCliente1 { get; set; }
    }
}
