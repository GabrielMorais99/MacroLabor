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
    
    public partial class TArquivoTransferencia
    {
        public System.Guid IdArquivoTransferencia { get; set; }
        public System.Guid IdIndice { get; set; }
        public System.Guid IdBlocoTransferencia { get; set; }
    
        public virtual TBlocoTransferencia TBlocoTransferencia { get; set; }
        public virtual TIndice TIndice { get; set; }
    }
}
