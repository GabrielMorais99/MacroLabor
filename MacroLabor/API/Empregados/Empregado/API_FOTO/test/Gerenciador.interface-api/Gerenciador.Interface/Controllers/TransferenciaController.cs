using Gerenciador.Negocio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Gerenciador.Interface.Controllers
{
    public class TransferenciaController : Controller
    { 
 
 
        [HttpGet]
        public ActionResult RetornarTransferencias(string parametro)
        {
            var dtInicio = new DateTime(Convert.ToInt32(parametro.Split('/')[1]), Convert.ToInt32(parametro.Split('/')[0]), 1);


            var lista = Repositorio.Retornar_Transferencias(dtInicio, dtInicio.AddMonths(1).AddDays(-1));

            return Json(new { transferencias = lista }, JsonRequestBehavior.AllowGet);
        }


        // GET: Transferencia
        public ActionResult Index()
        {
            var dtTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var dtInicio = dtTermino.AddMonths(-12);

            Dictionary<string, string> filtro = new Dictionary<string, string>();

            for (DateTime d = dtTermino; d > dtInicio; d = d.AddMonths(-1))

                filtro.Add(string.Format("{0}/{1}", d.Month, d.Year), string.Format("{0}/{1}", d.Month, d.Year));


            ViewBag.MesAno = filtro;

            return View(Repositorio.Retornar_Transferencias(new DateTime(DateTime.Now.Year,DateTime.Now.Month,1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1)));
        }
    }
}