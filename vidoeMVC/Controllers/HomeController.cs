using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace vidoeMVC.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {

            return View();
        }    
        
    }

}
