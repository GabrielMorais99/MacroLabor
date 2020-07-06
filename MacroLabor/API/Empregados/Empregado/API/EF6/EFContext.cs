using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace EF6
{
    public class EFContext : DbContext
    {

        public EFContext(string conexao): base(conexao)
        {
            Main.Principal();
        }

            public EFContext() { 


            Guid idFoto = Guid.Parse("9b2445fe-ae4a-4d04-ad37-3bc3c275e65e");
            var dadosRetornoRepositorio = Gerenciador.Negocio.Arquivo.Obter_Arquivo(idFoto);

        }
    }
}
