using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class TRepositorio
    {
        public TRepositorio()
        {
            this.TRepositSistemaCliente = new HashSet<TRepositSistemaCliente>();
        }

        public System.Guid IDRepositorio { get; set; }
        public string NomeBaseDados { get; set; }
        public string DadosConexao { get; set; }
        public bool Historica { get; set; }
        public bool ApenasLeitura { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRepositSistemaCliente> TRepositSistemaCliente { get; set; }
    }
}
