using Microsoft.AspNetCore.Mvc;

namespace vidoeMVC.Controllers
{
    public class SubscriptionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
