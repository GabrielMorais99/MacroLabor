using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public class IndiceDto
    {
        public Guid IdIndice { get; set; }

        public Guid IdArquivo { get; set; }

        public Guid IdRepositorioSistemaCliente { get; set; }
    }
}
