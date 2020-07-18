using System;
using System.Collections.Generic;
using System.Text;

namespace Repositorio.Dal
{
    public partial class Arquivo
    {
        public System.Guid IdArquivo { get; set; }
        public int IdRepositorio { get; set; }
        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string Metadado { get; set; }
        public decimal TamanhoBytes { get; set; }
        public byte[] Arquivo1 { get; set; }
        public System.DateTime Registro { get; set; }
        public Nullable<System.DateTime> Alteracao { get; set; }
        public bool Ativo { get; set; }

        public virtual Repositorio Repositorio { get; set; }
    }
}
