using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iWasHere.Web.Models;
using iWasHere.Domain.Models;
using iWasHere.Domain.Service;
using iWasHere.Domain.DTOs;

namespace iWasHere.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //updatebutton
        [HttpPost]
        public IActionResult LandmarkSubmit(LandmarkModel landmark, string btnSave)
        {
            switch (btnSave)
            {
                case "Save":
                    _homeService.UpdateLandmark(landmark, out string errorMessage2);
                    if (!string.IsNullOrEmpty(errorMessage2))
                    {
                        TempData["message"] = errorMessage2;
                        return RedirectToAction("AddEditNewLandmark", new { id = landmark.LandmarkId });
                    }
                    return Redirect("/Dictionary/Landmark");
                case "Save and New":
                    _homeService.UpdateLandmark(landmark, out string errorMessage);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData["message"] = errorMessage;
                        return RedirectToAction("AddEditNewLandmark", new { id = landmark.LandmarkId });
                    }
                    return Redirect("/Dictionary/Landmark");
                default:
                    return Redirect("/Dictionary/Landmark");
            }
        }
    }
}
