using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class TSistema
    {
        public TSistema()
        {
            this.TSistemaCliente = new HashSet<TSistemaCliente>();
        }

        public System.Guid IDSistema { get; set; }
        public string Sistema { get; set; }
        public bool Ativo { get; set; }
        public System.DateTime DataCriacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TSistemaCliente> TSistemaCliente { get; set; }
    }
}
