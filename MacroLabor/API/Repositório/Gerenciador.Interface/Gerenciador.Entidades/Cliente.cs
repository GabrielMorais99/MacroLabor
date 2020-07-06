using System;
using System.ComponentModel.DataAnnotations;

namespace Gerenciador.Entidades
{
    public class Cliente
    {
        [Display(Name = "Id")]
        public Guid IdCliente { get; set; }

        public bool Ativo { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Data Cadastro")]
        public DateTime  DataCadastro { get; set; }

    }
}
