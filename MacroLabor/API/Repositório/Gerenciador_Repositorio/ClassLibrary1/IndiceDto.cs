using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerenciador.Dal
{
   public  class IndiceDto
    {
        public Guid IdIndice { get; set; }

        public Guid IdArquivo { get; set; }

        public Guid IdRepositorioSistemaCliente { get; set; }

    }
}
