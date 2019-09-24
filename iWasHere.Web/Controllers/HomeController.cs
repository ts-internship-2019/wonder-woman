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
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using iWasHere.Domain.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace iWasHere.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;
        private IHostingEnvironment _environment;
     
        public HomeController(HomeService homeService, IHostingEnvironment environment)
        {
            _homeService = homeService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddEditNewLandmark(int id)
        {
            if(id != 0)
            {
                LandmarkModel model = _homeService.GetLandmarkById(id);
                ViewData["Images"] = _homeService.GetImagesForLandmarkId(id);
                return View(model);
            }
            return View(new LandmarkModel());
        }
        //public IActionResult AddEditNewLandmark(int id,string name="")
        //{
        //    Landmark landmark = new Landmark();
        //    if (!string.IsNullOrEmpty(name))
        //    {

        //        landmark.LandmarkId = id;

        //    }

        //    return View(landmark);
        //}

        public IActionResult Landmark_Read(int id)
        {
            LandmarkModel model = _homeService.GetLandmarkById(id);
            model.MapUrl = "https://www.google.com/maps/embed/v1/place?q=" + model.Latitude.ToString() + "," + model.Longitude.ToString() + "&amp;&key=AIzaSyC0vB7-K0LOaHIDEGEgHba6Wo2f099UFvE";

            ViewData["Images"] = _homeService.GetImagesForLandmarkId(id);
            ViewData["Location"] = _homeService.GetLocationForLandmarkId(id);
            ViewData["Construction"] = _homeService.GetConstructionForLandmarkId(id); 
            ViewData["Landmark"] = _homeService.GetLandmarktypeForLandmarkId(id);
            ViewData["Comment"] = _homeService.GetCommentsForLandmarkId(id);
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


        public IActionResult Images(int id,string name="")
        {
            Landmark landmark = new Landmark();
            if(!string.IsNullOrEmpty(name)) {
          
                landmark.LandmarkId = id;
           
            }
         
            return View(landmark);
       
        }

        public void SubmitImage(List<IFormFile> files,int LandmarkId)
        {
            List<string> path = new List<string>();
            foreach (var image in files)
            {

                if (image.Length > 0)
                {
                    //var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);

                    var a = Guid.NewGuid().ToString();
                    var fileName = Path.Combine(_environment.WebRootPath + "/images", a + Path.GetExtension(image.FileName));
                    image.CopyTo(new FileStream(fileName, FileMode.Create));
                    path.Add(a + Path.GetExtension(image.FileName));

                }

            }
            foreach (string p in path)
            {
                _homeService.SaveImagesDB(p,LandmarkId);

            }

            //  return RedirectToAction("Edit", new { id = employee.Id,name=employee.FirstName});



        }


        //updatebutton
        [HttpPost]
        public IActionResult LandmarkSubmit(LandmarkModel landmark, string btnSave, List<IFormFile> files)
        {
            switch (btnSave)
            {
              
                case "Save":
                    _homeService.UpdateLandmark(landmark, out string errorMessage2,out int id);
                    SubmitImage(files, id);
                    if (!string.IsNullOrEmpty(errorMessage2))
                    {
                        TempData["message"] = errorMessage2;
                        return RedirectToAction("AddEditNewLandmark", new { id = landmark.LandmarkId });
                    }
                    return Redirect("/Home/Landmarks_List_Read");
                case "Save and New":
                    _homeService.UpdateLandmark(landmark, out string errorMessage,out int id2);
                    SubmitImage(files, id2);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData["message"] = errorMessage;
                        return RedirectToAction("AddEditNewLandmark", new { id = landmark.LandmarkId });
                    }
                    return Redirect("/Home/AddEditNewLandmark");
                default:
                    return Redirect("/Home/Landmarks_List_Read");
            }
        }

        public JsonResult Landmarks_Read_ForCB(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }

            List<DictionaryLandmarkType> list = GetLandmarksForCB(text);

            return Json(list);
        }

        public List<DictionaryLandmarkType> GetLandmarksForCB(string text)
        {
            List<DictionaryLandmarkType> landmarks = _homeService.GetLandmarks(text);
            return landmarks;
        }

        public JsonResult Cities_Read_ForCB(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }

            List<CityModel> list = GetCountriesForCB(text);

            return Json(list);
        }

        public List<CityModel> GetCountriesForCB(string text)
        {
            List<CityModel> cityModels = _homeService.GetCities(text);
            return cityModels;
        }

        public JsonResult Constructions_Read_ForCB(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }

            List<DictionaryConstructionTypeModel> list = GetConstructionsForCB(text);

            return Json(list);
        }

        public List<DictionaryConstructionTypeModel> GetConstructionsForCB(string text)
        {
            List<DictionaryConstructionTypeModel> constructionModels = _homeService.GetConstructions(text);
            return constructionModels;
        }

        public ActionResult DestroyPhoto([DataSourceRequest] DataSourceRequest request, string photoPath)
        {
            System.IO.File.Delete(photoPath);
            return Json(request);
        }

        [HttpPost]
        public IActionResult CommentSubmit(LandmarkModel ldm)
        {
            Comment comm = new Comment()
            {
                LandmarkId = ldm.LandmarkId,
                Text = ldm.CommentText
            };
            _homeService.AddComments(comm, out string errorMessage2);
            if (!string.IsNullOrEmpty(errorMessage2))
            {
                TempData["message"] = errorMessage2;
            }
            return RedirectToAction("Landmark_Read", new { id = comm.LandmarkId });
        }
    }
}

