using Microsoft.AspNetCore.Mvc;

namespace ExamenWeb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
