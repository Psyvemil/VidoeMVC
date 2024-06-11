using Microsoft.AspNetCore.Mvc;

namespace vidoeMVC.Controllers
{
    public class VideoUploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
