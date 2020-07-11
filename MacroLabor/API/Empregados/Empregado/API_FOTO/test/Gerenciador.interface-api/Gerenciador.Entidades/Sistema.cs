using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gerenciador.Entidades
{
    public class Sistema
    {
        public Sistema()
        {
            SistemasClientes = new List<SistemaCliente>();
        }

        [Display(Name = "Id")]
        public Guid IdSistema { get; set; }

        public bool Ativo { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Data Cadastro")]
        public DateTime DataCadastro { get; set; }

        public IEnumerable<SistemaCliente>  SistemasClientes { get; set; }

    }
}
