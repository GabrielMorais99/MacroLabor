using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repositorio.Dal.Model
{
    [Table("TGEG_EmpregadoEndereco")]

    public class EnderecoEmpregado
    {
        [Key]
        public Guid IdEmpregadoEndereco { get; set; }
        public int IdEmpregado { get; set; }
        public int Cep { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int IdModoTransporte { get; set; }
    }
}
