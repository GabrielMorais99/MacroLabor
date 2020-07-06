using Gerenciador.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador_Repositorio.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           // var o = new Arquivo(new Guid("86034e99-7a6c-4a42-82d8-d9fb82e8fa19")); //Vigente A - Aceite

            var o = new Arquivo(new Guid("c732f3a4-07ed-4153-b927-0000120c4a20")); //  - Vigente B 

            byte[] array = Enumerable.Repeat((byte)0x20,32).ToArray();


            o.Gravar("TESTE_FINAL1.img", array);


            o = new Arquivo(new Guid("c732f3a4-07ed-4153-b927-0000120c4a20")); // Producao - Vigente B


            o.Gravar("TESTE_FINAL2.img", array);


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}