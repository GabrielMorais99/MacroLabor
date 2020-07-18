using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public class RepositorioDto
    {
        public Guid IdRepositorioClienteSistema { get; set; }

        public Guid IdRepositorio { get; set; }

        public string NomeBaseDados { get; set; }

        public string StringConexao { get; set; }

        public Guid IdSistemaCliente { get; set; }

        public bool Historica { get; set; }

        public bool ApenasLeitura { get; set; }

        public int PrazoRetencao { get; set; }

    }
}
