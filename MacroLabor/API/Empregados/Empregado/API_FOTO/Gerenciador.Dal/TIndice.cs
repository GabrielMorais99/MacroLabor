using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class TIndice
    {
        public System.Guid IDIndice { get; set; }
        public System.Guid IDArquivo { get; set; }
        public System.Guid IDRepositorioSistemaCliente { get; set; }
        public System.DateTime DataCriacao { get; set; }

        public virtual TRepositSistemaCliente TRepositSistemaCliente { get; set; }
    }
}
