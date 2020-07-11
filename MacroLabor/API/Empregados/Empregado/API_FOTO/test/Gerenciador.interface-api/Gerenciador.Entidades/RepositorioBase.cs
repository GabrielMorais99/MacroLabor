using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gerenciador.Entidades
{
    public class RepositorioBase
    {
        public RepositorioBase()
        { 
            RepositoriosSistemaCliente = new List<Repositorio>();
        }

        public Guid IdRepositorio { get; set; } 

        [Display(Name = "String Conexão")]
        public string StringConexao { get; set; }

        public string Servidor { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Usuário")]
        public string Usuario { get; set; }

        public string Senha { get; set; }

        [Display(Name = "Histórica")]
        public bool Historica { get; set; }

        [Display(Name = "Histórica")]
        public string Historica_ { get { return (Historica) ? "Sim" : "Não"; } }

        [Display(Name = "Apenas Leitura")]
        public bool ApenasLeitura { get; set; }

        [Display(Name = "Apenas Leitura")]
        public string ApenasLeitura_ { get { return (ApenasLeitura) ? "Sim" : "Não"; } }

        public IEnumerable<Repositorio> RepositoriosSistemaCliente { get; set; }

        public IEnumerable<SistemaCliente> SistemasClientes { get; set; }

        public virtual bool Ativo { get; set; } 

    }
}
