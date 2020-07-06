using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6
{
    public class Main
    {

        public static void Principal()
        {
            Guid idFoto = Guid.Parse("9b2445fe-ae4a-4d04-ad37-3bc3c275e65e");
            Gerenciador.Entidades.Arquivo foto  = Gerenciador.Negocio.Arquivo.Obter_Arquivo(idFoto);
        }
    }
}
