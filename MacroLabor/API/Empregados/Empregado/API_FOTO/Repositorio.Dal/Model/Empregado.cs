using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Repositorio.Dal.Model
{
    [Table("TEmpregado")]

    public class Empregado
    {
        [Key]
        public int IDEmpregado { get; set; }
        public Guid idFoto { get; set; }
        public string Atividade { get; set; }
        public string Operacao { get; set; }
        public int Matricula { get; set; }
        public string Cargo { get; set; }
        public string CPF { get; set; }
        public string Empresa { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public EnderecoEmpregado EmpregadoEndereco { get; set; }
        public EmpregadoFoto EmpregadoFoto { get; set; }

    }

    public static class Roles
    {
        public const string ROLE_API_EMPREGADO = "Acesso-APIEmpregado";
    }
}
