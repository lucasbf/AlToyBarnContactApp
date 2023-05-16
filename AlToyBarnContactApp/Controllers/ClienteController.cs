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
            return View(_service.FindAll());
        }
    }
}
