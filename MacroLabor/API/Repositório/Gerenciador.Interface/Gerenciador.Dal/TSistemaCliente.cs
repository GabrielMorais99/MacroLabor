//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gerenciador.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class TSistemaCliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TSistemaCliente()
        {
            this.TRepositSistemaCliente = new HashSet<TRepositSistemaCliente>();
        }
    
        public System.Guid IDSistemaCliente { get; set; }
        public System.Guid IDSistema { get; set; }
        public System.Guid IDCliente { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public bool Ativo { get; set; }
        public string Nome { get; set; }
    
        public virtual TCliente TCliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRepositSistemaCliente> TRepositSistemaCliente { get; set; }
        public virtual TSistema TSistema { get; set; }
    }
}
