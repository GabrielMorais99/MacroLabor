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
    
    public partial class TIndice
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIndice()
        {
            this.TArquivoTransferencia = new HashSet<TArquivoTransferencia>();
        }
    
        public System.Guid IDIndice { get; set; }
        public System.Guid IDArquivo { get; set; }
        public System.Guid IDRepositorioSistemaCliente { get; set; }
        public System.DateTime DataCriacao { get; set; }
        public string NomeArquivo { get; set; }
        public System.DateTime DataUltimaAlteracao { get; set; }
        public bool Ativo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TArquivoTransferencia> TArquivoTransferencia { get; set; }
    }
}
