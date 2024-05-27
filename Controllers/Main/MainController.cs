using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IT15_Project.Controllers.Main
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View("MainContent");
        }
    }
}
