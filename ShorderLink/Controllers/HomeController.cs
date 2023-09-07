using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShorderLink.Identity;
using ShorderLink.Models;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace ShorderLink.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string AboutText = "This web service created to make Long URL contstant length of 16 elements.\r\n" +
            "To achive this goal used hash algoritm SHA256 wich transform URL into unique Hash code.\r\n" +
            "Then using the % (modulus) operator, we select a char for each byte.\r\n" +
            "All Links saved in DataBase and when user try to get something by short link, server automaticaly redirect to original link.";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {

            if (TempData["AboutText"] != null)
            {
                ViewBag.AboutText = TempData["AboutText"] as string;
            }
            else ViewBag.AboutText = AboutText;
            return View();
        }

        /// <summary>
        /// Get data to show in About view
        /// </summary>
        /// <param name="text">String o show in About</param>
        /// <returns>Redirect to About with new data</returns>
        [HttpPost]
        public IActionResult ChangeAbout([FromForm] string text)
        {
            
            TempData["AboutText"] = text;
            ViewBag.AboutText = text;
            
            return RedirectToAction("About");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}