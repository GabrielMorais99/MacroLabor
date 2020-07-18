using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repositorio.Dal.Model
{
    [Table("TIndice")]

    public class EmpregadoFoto
    {
        [Key]
        public Guid IdIndice { get; set; }
        public Guid IdFoto { get; set; }
        public byte[] BytesArquivo { get; set; }
    }
}
