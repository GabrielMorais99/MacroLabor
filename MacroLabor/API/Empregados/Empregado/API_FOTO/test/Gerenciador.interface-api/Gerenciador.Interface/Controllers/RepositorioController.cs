using Gerenciador.Negocio;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Gerenciador.Interface.Controllers
{
    public class RepositorioController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        // GET: RepositorioBase
        public ActionResult Retornar_Repositorios()
        {
            return Json(Repositorio.Retornar_Repositorios().Where(x => x.Ativo).OrderBy(x=>x.Historica).ThenBy(x=>x.Nome),JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Retornar_Sistemas_Clientes(string idRepositorioClienteSistema)
        {
            Guid result;

            if(Guid.TryParse(idRepositorioClienteSistema, out result))
                return Json(Repositorio.Retornar_Sistemas_Clientes(result).OrderBy(x => x.Nome), JsonRequestBehavior.AllowGet);

            return Json(Repositorio.Retornar_Sistemas_Clientes().OrderBy(x => x.Nome), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Retornar_Transferencias()
        {
            return Json(Repositorio.Retornar_Transferencias(DateTime.Now,DateTime.Now).OrderBy(x => x.DataInicioProcessamento), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Salvar(Entidades.RepositorioBase entidade)
        {
            try
            {
                entidade.IdRepositorio = Repositorio.Salvar(entidade);

                return Json(new { Repositorio = entidade, msg = "Repositório criado com sucesso!", erro = false  }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            { 
                return Json(new {   msg = ex.Message , erro = true }, JsonRequestBehavior.AllowGet);
            } 
        }
         
        public ActionResult Transferir(string id)
        {
            try
            {
                
                var retorno = Repositorio.Transferir(new Guid[] { Guid.Parse(id) });

                if(retorno)
                    return Json(new { msg = "Existem falhas na transferência de arquivos, verifique o log de transferências para obter mais informações !", success = false }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { msg = "Transferência de arquivos realizada com sucesso!", success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detalhe(Guid id)
        {
            return PartialView(Repositorio.Retornar_Repositorio(id));
        }
 
        public ActionResult Excluir(string id)
        {
            try
            {
                Repositorio.Excluir(Guid.Parse(id));

                return Json(new { msg = "Repositório excluído com sucesso!", erro = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = ex.Message, erro = true }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}