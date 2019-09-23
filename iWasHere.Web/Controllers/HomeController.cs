using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using iWasHere.Web.Models;
using iWasHere.Domain.Service;
using Kendo.Mvc.UI;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;

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

        public IActionResult AddEditNewLandmark()
        {
            return View();
        }

        public IActionResult Landmark_Read()
        public IActionResult Landmark_Read(int id)
        {
            LandmarkModel model = new LandmarkModel();
            model.Latitude = 40.7127837m;
            model.Longitude = -74.0059413m;
            model.MapUrl = "https://www.google.com/maps/embed/v1/place?q=" + model.Latitude.ToString() + "," + model.Longitude.ToString() + "&amp;&key=AIzaSyC0vB7-K0LOaHIDEGEgHba6Wo2f099UFvE";
            return View(model);
        }

        public ActionResult GetLandmarks([DataSourceRequest] DataSourceRequest request)
        {
            List<LandmarkListModel> landmarkList = _homeService.GetLandmarkListModels();
            DataSourceResult result = new DataSourceResult();
            result.Data = landmarkList;
            return Json(result);
        }
            public IActionResult Landmarks_List_Read([DataSourceRequest] DataSourceRequest request)
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
                    return Redirect("/Home/Landmark");
                case "Save and New":
                    _homeService.UpdateLandmark(landmark, out string errorMessage);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData["message"] = errorMessage;
                        return RedirectToAction("AddEditNewLandmark", new { id = landmark.LandmarkId });
                    }
                    return Redirect("/Home/Landmark");
                default:
                    return Redirect("/Home/Landmark");
            }
        }

        public JsonResult Landmarks_Read_ForCB(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }

            List<LandmarkModel> list = GetLandmarksForCB(text);

            return Json(list);
        }
        public List<LandmarkModel> GetLandmarksForCB(string text)
        {
            List<LandmarkModel> landmarks = _homeService.GetLandmarks(text);
            return landmarks;
        }
    }
}
