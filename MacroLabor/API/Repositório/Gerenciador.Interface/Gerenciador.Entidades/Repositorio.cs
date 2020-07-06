using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gerenciador.Entidades
{
    /// <summary>
    /// Repositório de arquivos segmentado por sistema/cliente.
    /// </summary>
    public  class Repositorio:RepositorioBase
    {

        public Repositorio()
        {
            Indices = new List<Indice>();
        }
        public Guid IdRepositorioClienteSistema { get; set; }

        public Guid IdSistemaCliente { get; set; }
       
        [Display(Name = "Prazo de Retenção")]
        public int? PrazoRetencao { get; set; }

        [Display(Name = "Prazo de Retenção")]
        public string PrazoRetencao_ { get { return (PrazoRetencao == null) ? string.Empty : string.Format("{0} dias", PrazoRetencao); } }

        public Guid IdSistema { get; set; }

        public string Sistema { get; set; }

        public Guid IdCliente { get; set; }

        public string Cliente { get; set; }

        public override bool Ativo { get; set; }

        [Display(Name = "Qtde. Arquivos")]
        public IEnumerable<Indice> Indices { get; set; } 

        [Display(Name = "Qtde. Arquivos")]
        public int Total_Arquivos { get { return Indices.Count(); } }

        [Display(Name = "Repositório Histórico")]
        public string Historico { get { return (Conexao_Historico == null)?string.Empty : Conexao_Historico.Split(';')[1].Split('=')[1]; } }

        public string Conexao_Historico { get; set; }

        public DateTime? LimiteMin { get; set; }

        public DateTime? LimiteMax { get; set; }





    }
}
