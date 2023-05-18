using AlToyBarnContactApp.Models;
using AlToyBarnContactApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlToyBarnContactApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteService _service;

        public ClienteController(IClienteService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            ICollection<Cliente> clientes = _service.FindAll();
            return View(clientes);
        }

        public IActionResult Detalhar(int id)
        {
            Cliente? cli = _service.Find(id);
            return View(cli);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Criar([Bind("Nome", "Endereco")] Cliente cliente)
        {
            if (Char.IsDigit(cliente.Nome[0]))
            {
                ModelState.AddModelError("Nome", "Nome não pode iniciar com dígito!");
                return View(cliente);
            }
            _service.Create(cliente);
            return RedirectToAction("Index");
        }
    }
}
