using Gerenciador.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gerenciador.Negocio
{
    public static class Cliente
    {
        public static Guid Salvar(Entidades.Cliente entidade) {
             

            if (Retornar_Clientes().Any(x => x.IdCliente != entidade.IdCliente && x.Ativo && x.Nome == entidade.Nome))
                throw new Exception("Cliente já cadastrado!");

            return Dal.Cliente.Salvar(entidade);


        }

        public static IEnumerable<Entidades.Cliente> Retornar_Clientes()
        {
            return Dal.Cliente.Retornar_Clientes();
        }

        public static IEnumerable<SistemaCliente> Retornar_Sistemas_Clientes()
        {
            var lista = Dal.Cliente.Retornar_Sistemas_Clientes();

            return lista;
        }

        public static void Excluir(Guid idCliente)
        {
            if (Retornar_Sistemas_Clientes().Any(x => x.Ativo && x.IdCliente == idCliente))
                throw new Exception("Existe sistemas viculados a este sistema!");

            Dal.Cliente.Excluir(idCliente);

        }

    }
}
