using Microsoft.AspNetCore.Mvc;
using vidoeMVC.ViewModels;

namespace vidoeMVC.Controllers
{
    public class VideoUploadController : Controller
    {
        public IActionResult Index()
        {
            var homewm =new HomeVM();
            return View(homewm);
        }
    }
}
