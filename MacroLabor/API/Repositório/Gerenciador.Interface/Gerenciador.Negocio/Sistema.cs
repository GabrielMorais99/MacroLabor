using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gerenciador.Negocio
{
    public  static class Sistema
    {
        public static Guid Salvar(Entidades.Sistema entidade)
        {


            if (Retornar_Sistemas().Any(x => x.IdSistema != entidade.IdSistema && x.Ativo && x.Nome == entidade.Nome))
                throw new Exception("Sistema já cadastrado!");

            return Dal.Sistema.Salvar(entidade);


        }

        public static IEnumerable<Entidades.Sistema> Retornar_Sistemas()
        {
            return Dal.Sistema.Retornar_Sistemas();
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes(Guid idSistema)
        {
            var lista = Dal.Sistema.Retornar_Sistemas_Clientes(idSistema);

            return lista;
        }

        public static void Excluir(Guid idSistema)
        {
            if (Retornar_Sistemas_Clientes(idSistema).Any(x => x.Ativo))
                throw new Exception("Existe clientes viculados a este sistema!");

            Dal.Sistema.Excluir(idSistema);

        }

      
 





    }
}
