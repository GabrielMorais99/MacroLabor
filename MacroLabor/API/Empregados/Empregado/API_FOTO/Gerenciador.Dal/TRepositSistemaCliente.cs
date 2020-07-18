using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class TRepositSistemaCliente
    {
        public TRepositSistemaCliente()
        {
            this.THistoricoTransferencia = new HashSet<THistoricoTransferencia>();
            this.THistoricoTransferencia1 = new HashSet<THistoricoTransferencia>();
            this.TIndice = new HashSet<TIndice>();
        }

        public System.Guid IDRepositSistemaCliente { get; set; }
        public System.Guid IDRepositorio { get; set; }
        public int PrazoRetencaoDias { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        public System.Guid IDSistemaCliente { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THistoricoTransferencia> THistoricoTransferencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THistoricoTransferencia> THistoricoTransferencia1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TIndice> TIndice { get; set; }
        public virtual TRepositorio TRepositorio { get; set; }
        public virtual TRepositSistemaCliente TRepositSistemaCliente1 { get; set; }
        public virtual TRepositSistemaCliente TRepositSistemaCliente2 { get; set; }
    }
}
