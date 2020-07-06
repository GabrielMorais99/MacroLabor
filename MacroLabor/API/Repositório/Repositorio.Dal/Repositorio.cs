//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repositorio.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class Repositorio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Repositorio()
        {
            this.Arquivo = new HashSet<Arquivo>();
        }
    
        public int IdRepositorio { get; set; }
        public int IdSistema { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ChavePrivada { get; set; }
        public string Tipo { get; set; }
        public System.DateTime Registro { get; set; }
        public Nullable<System.DateTime> Alteracao { get; set; }
        public bool Ativo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Arquivo> Arquivo { get; set; }
        public virtual Sistema Sistema { get; set; }
    }
}
