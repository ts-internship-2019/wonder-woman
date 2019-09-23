using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iWasHere.Web.Models;
using iWasHere.Domain.DTOs;

namespace iWasHere.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Landmark_Read()
        {
            LandmarkModel model = new LandmarkModel();
            model.MapUrl = "https://www.google.com/maps/embed/v1/directions?origin=1%20Foxfield%20Lawn2C%20Raheny%2C%20Ireland&destination=128%20Old%20County%20Road%2C%20Crumlin%2C%20Ireland&key=AIzaSyC0vB7-K0LOaHIDEGEgHba6Wo2f099UFvE";
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
