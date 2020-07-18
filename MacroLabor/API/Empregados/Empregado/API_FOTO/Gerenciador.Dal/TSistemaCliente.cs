using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class TSistemaCliente
    {
        public System.Guid IDSistemaCliente { get; set; }
        public System.Guid IDSistema { get; set; }
        public System.Guid IDCliente { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }

        public virtual TCliente TCliente { get; set; }
        public virtual TSistema TSistema { get; set; }
    }
}
