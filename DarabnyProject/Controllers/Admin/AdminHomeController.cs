using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DarabnyProject.Controllers.Admin
{
    [Authorize(Roles = "Admin,Teacher")]
        public class AdminHomeController : Controller
    {
        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
