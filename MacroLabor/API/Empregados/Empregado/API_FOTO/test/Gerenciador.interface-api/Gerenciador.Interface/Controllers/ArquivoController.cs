using Gerenciador.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gerenciador.Interface.Controllers
{
    public class ArquivoController : Controller
    {
        // GET: Gerenciador
        public ActionResult Index()
        {
            ViewBag.Repositorios = Arquivo.Retornar_Repositorios().Where(x=>x.Ativo).OrderBy(x=>x.Nome);

            return View();
        }
    
        public ActionResult Obter_Clientes_Sistemas(string idRepositorio)
        {
            var retorno = Arquivo.Retornar_Sistemas_Clientes(new Guid(idRepositorio));

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Importar()
        {
            try
            {

                if (HttpContext.Request.Form["Arquivo"] != null)
                {
                    var indice = HttpContext.Request.Form["hdnIndice"] == string.Empty ? null : (Guid?)new Guid(HttpContext.Request.Form["hdnIndice"]);

                    var nomeArquivo = HttpContext.Request.Form["NomeArquivo"];

                    var arquivo = Convert.FromBase64String(HttpContext.Request.Form["Arquivo"]);

                    var sistemaCliente = HttpContext.Request.Form["SistemaCliente"];

                    var repositorio = HttpContext.Request.Form["Repositorio"];

                    var entidade = (indice == Guid.Empty || indice == null) ? Arquivo.Incluir(nomeArquivo, arquivo, new Guid(repositorio),new Guid(sistemaCliente)) : Arquivo.Atualizar(nomeArquivo, arquivo, (Guid)indice);

                    return Json(new
                    {
                        Success = true,
                        arquivo = new{nome = entidade.NomeArquivo,repositorio = entidade.Repositorio,tamanho = entidade.Tamanho, dataCadastro = entidade.DataCadastro.ToString(),indice = entidade.Indice},
                        Mensagem = "Arquivo salvo com sucesso!"
                    }, JsonRequestBehavior.AllowGet);
                }

                return null;
                
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }

        } 

        public ActionResult Obter_Arquivo(string indice)
        {
            try
            {

                if (new Guid(indice) == Guid.Empty)
                    throw new Exception("Não foi possível realizar a busca deste arquivo pois a referência para a pesquisa é nula!");

                Guid indice_ = Guid.Empty;

                JsonResult jsonResult;

                if (Guid.TryParse(indice, out indice_))
                {
                    var entidade = Arquivo.Obter_Arquivo(indice_);
                
                     jsonResult = Json(new { Success = true, Mensagem = "Arquivo encontrado com sucesso!", Entidade = entidade }, JsonRequestBehavior.AllowGet);

                    jsonResult.MaxJsonLength = int.MaxValue;

                    return jsonResult;

                }
                else
                {
                    throw new Exception("O índice repassado não está no formato GUID!");
                }

            }
            catch (Exception e)
            {

                return Json(new { Success = false, Mensagem = e.Message}, JsonRequestBehavior.AllowGet);
            } 
        }
        [HttpGet]
        public ActionResult ObterArquivos(string idRepositorio,string idSistemaCliente,string filtro, int page, int take)
        {
            try
            {
                Guid idSistemaCliente_ = Guid.Empty;

                Guid idRepositorio_ = Guid.Empty;

                JsonResult jsonResult = new JsonResult() ;

                if (Guid.TryParse(idSistemaCliente, out idSistemaCliente_) && Guid.TryParse(idRepositorio,out idRepositorio_))
                {
                    var entidades = Arquivo.Obter_Arquivos(idRepositorio_,idSistemaCliente_,filtro, page,take);
    
                    jsonResult = Json( new { Sucesso = true, Arquivos = entidades}, JsonRequestBehavior.AllowGet);

                    jsonResult.MaxJsonLength = int.MaxValue;

                    return jsonResult;

                }

                return Json(string.Empty, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { Sucesso = false, Mensagem= e.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Excluir_Arquivo(string indice)
        {
            try
            {
                Guid indice_ = Guid.Empty;

                if (Guid.TryParse(indice, out indice_))
                    Arquivo.Excluir_Arquivo(indice_);
                else

                    return Json("Indice inválido!", JsonRequestBehavior.AllowGet);
                        
               return Json(new { Mensagem = "Arquivo excluído com sucesso!", Success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json( new { Mensagem = e.Message, Success= false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}