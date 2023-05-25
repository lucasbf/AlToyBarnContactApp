using AlToyBarnContactApp.Models;
using AlToyBarnContactApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlToyBarnContactApp.Controllers
{
    public class ContatoController : Controller
    {
        private readonly IContatoService _serviceContato;

        private List<SelectListItem> _redesSociais = new()
        {
            new SelectListItem() { Value = "Instagram", Text = "Instagram" },
            new SelectListItem() { Value = "TikTok", Text = "TikTok" },
            new SelectListItem() { Value = "Twitter", Text = "Twitter"},
            new SelectListItem() { Value = "Email", Text = "Email"}
        };
        public ContatoController(IContatoService serviceContato)
        {
            _serviceContato = serviceContato;
        }

        public IActionResult Index(int id)
        {
            var contatos = _serviceContato.FindFromCliente(
                new Models.Cliente { Id = id });
            return View(contatos);
        }

        public IActionResult Criar(int id)
        {
            ViewBag.Id = id;
            ViewBag.RedesSociais = _redesSociais;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Criar([Bind("IdCliente","Perfil","Tipo")] Contato contato)
        {
            _serviceContato.Create(contato);
            return RedirectToAction("Index", new { id = contato.IdCliente});
        }

        [HttpPost]
        public IActionResult Remover(int id, int idcliente)
        {
            var contato = _serviceContato.Find(new Contato { Id = id, IdCliente = idcliente })!;
            _serviceContato.Delete(contato);
            return Json(new { sucess = true });
        }
    }
}
