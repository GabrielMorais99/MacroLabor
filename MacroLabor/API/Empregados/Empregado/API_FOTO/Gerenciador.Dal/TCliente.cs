using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class TCliente
    {
        public TCliente()
        {
            this.TSistemaCliente = new HashSet<TSistemaCliente>();
        }

        public string Cliente { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        public System.Guid IDCliente { get; set; }

        public virtual ICollection<TSistemaCliente> TSistemaCliente { get; set; }
    }
}
