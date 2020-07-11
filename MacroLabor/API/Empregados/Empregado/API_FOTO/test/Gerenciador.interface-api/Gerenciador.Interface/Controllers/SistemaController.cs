using Gerenciador.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Interface.Controllers
{
    public class SistemaController : Controller
    {
        public ActionResult Index()
        {
           

            return View();
        }

        public ActionResult Retornar_Sistemas()
        {
            return Json(Sistema.Retornar_Sistemas().Where(x=>x.Ativo).OrderBy(x => x.Nome), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Retornar_Clientes()
        {
            return Json(Cliente.Retornar_Clientes().Where(x => x.Ativo).OrderBy(x => x.Nome), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Salvar(Entidades.Sistema entidade)
        {
            try
            {
                Sistema.Salvar(entidade);

                return Json(new { Sistema = entidade, msg = "Sistema salvo com sucesso!", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { msg = e.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult Excluir(string id)
        {
            try
            {
                Sistema.Excluir(Guid.Parse(id));

                return Json(new { msg = "Sistema excluído com sucesso!", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }

    
 
      

    }
}
