using System;
using System.ComponentModel.DataAnnotations;

namespace test.Entidades
{
    public class Arquivo
    {
         [Display(Name = "Índice")]
        public Guid Indice { get; set; }

        public Guid IdArquivo { get; set; }

        public Guid IdRepositorioSistemaCliente { get; set; }

        public Guid IdRepositorio { get; set; }

        public Guid IdSistemaCliente { get; set; }

        public string Cliente { get; set; }

        public string Sistema { get; set; }

        [Display(Name = "Nome")]
        public string NomeArquivo { get; set; }

        public decimal Tamanho { get; set; }

        public byte[] Conteudo { get; set; }

        [Display(Name = "Data Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Data Cadastro")]
        public String DataCadastro_ { get { return DataCadastro.ToString(); } }

        [Display(Name = "Repositório")]
        public string Repositorio { get; set; }

        [Display(Name = "Conexão")]
        public string StringConexao { get; set; }

        [Display(Name = "Histórico")]
        public bool Historico { get; set; }

        public bool Ativo { get; set; }

    }
}