using System;

namespace Gerenciador.Entidades
{
    public class Indice
    {
        public Guid IdIndice { get; set; }

        public Guid IdArquivo { get; set; }

        public Guid IdRepositorioSistemaCliente { get; set; }

        public string NomeArquivo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

    }
}
