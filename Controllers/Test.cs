using Microsoft.AspNetCore.Mvc;

namespace IT15_Project.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
