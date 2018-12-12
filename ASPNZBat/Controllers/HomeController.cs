using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNZBat.Models;

namespace ASPNZBat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Create", "SeatBookings");
            //var result = DependencyResolver.Current.GetService<SeatBookingsController>();
            //return RedirectToAction(SeatBookingsController, "Create");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return RedirectToAction("Create", "SeatBookings");
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return RedirectToAction("Create", "SeatBookings");
        }

        public IActionResult Privacy()
        {
            return RedirectToAction("Create", "SeatBookings");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
