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
    
    public partial class Sistema
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sistema()
        {
            this.Repositorio = new HashSet<Repositorio>();
        }
    
        public int IdSistema { get; set; }
        public string Nome_do_Sistema { get; set; }
        public string Descricao_do_Sistema { get; set; }
        public string ChavePrivada { get; set; }
        public System.DateTime Registro { get; set; }
        public Nullable<System.DateTime> Alteracao { get; set; }
        public bool Ativo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Repositorio> Repositorio { get; set; }
    }
}
