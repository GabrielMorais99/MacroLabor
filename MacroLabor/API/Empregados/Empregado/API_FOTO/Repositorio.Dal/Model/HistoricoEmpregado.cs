using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repositorio.Dal.Model
{
    [Table("TEmpregadoMovimentacao")]

    public  class HistoricoEmpregado
    {
        public string Empresa { get; set; }
        public string Operacao { get; set; }
        public string Atividade { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string Tipo { get; set; }
        public string Cargo { get; set; }
    }
}
