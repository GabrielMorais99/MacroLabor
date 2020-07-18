using System;
using System.Collections.Generic;
using System.Text;

namespace Repositorio.Dal
{
    public class ArquivoDto
    {

        public Guid IdArquivo { get; set; }

        public string NomeArquivo { get; set; }

        public decimal Tamanho { get; set; }

        public byte[] Arquivo { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }
    }
}
