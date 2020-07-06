using Gerenciador.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Interface.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View( );
        }

        [HttpGet]
        public ActionResult Retornar_Clientes()
        {
            return Json(Cliente.Retornar_Clientes().OrderBy(x => x.Nome), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Salvar(Entidades.Cliente entidade)
        {
            try
            {
                Cliente.Salvar(entidade);

                return Json(new { Cliente = entidade, msg = "Cliente salvo com sucesso!", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { msg= e.Message , erro= true}, JsonRequestBehavior.AllowGet);
            }

          
        }

        public ActionResult Excluir(string id)
        {
            try
            {
                Cliente.Excluir(Guid.Parse(id));

                return Json(new { msg = "Cliente excluído com sucesso!", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
