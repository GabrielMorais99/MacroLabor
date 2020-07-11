using System;

namespace Gerenciador.Entidades
{
    public class SistemaCliente
    {
        public Guid IdSistemaCliente { get; set; }

        public Guid IdCliente { get; set; }

        public Guid IdSistema { get; set; }

        public string Nome { get; set; }

        public string Sistema { get; set; }

        public string Cliente { get; set; }

        public bool Ativo { get; set; }

        public int? PrazoRetencao { get; set; }

        public DateTime DataCadastro { get; set; }

    }
}
