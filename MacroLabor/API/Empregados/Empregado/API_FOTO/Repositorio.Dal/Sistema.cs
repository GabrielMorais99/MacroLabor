using System;
using System.Collections.Generic;
using System.Text;

namespace Repositorio.Dal
{
    public partial class Sistema
    {
        public Sistema()
        {
            this.Repositorio = new HashSet<Repositorio>();
        }

        public int IdSistema { get; set; }
        public string Nome_do_Sistema { get; set; }
        public string Descricao_do_Sistema { get; set; }
        public string ChavePrivada { get; set; }
        public System.DateTime Registro { get; set; }
        public Nullable<System.DateTime> Alteracao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Repositorio> Repositorio { get; set; }
    }
}
