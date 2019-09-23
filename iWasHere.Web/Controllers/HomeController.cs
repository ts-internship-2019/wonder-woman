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

        public IActionResult Landmark_Read()
        {
            LandmarkModel model = new LandmarkModel();
            model.MapUrl = "https://www.google.com/maps/embed/v1/directions?origin=1%20Foxfield%20Lawn2C%20Raheny%2C%20Ireland&destination=128%20Old%20County%20Road%2C%20Crumlin%2C%20Ireland&key=AIzaSyC0vB7-K0LOaHIDEGEgHba6Wo2f099UFvE";
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


        public JsonResult Landmarks_Read_ForCB(string text)
        {

         
            List<LandmarkListModel> list = _homeService.GetLandmarkListModels(); 

            return Json(list);
        }
        public IActionResult Images(int id,string name="")
        {
            Landmark landmark = new Landmark();
            if(!string.IsNullOrEmpty(name)) {
          
                landmark.LandmarkId = id;
            }
         
            return View(landmark);
       
        }

        public ActionResult SubmitImage(List<IFormFile> files,int LandmarkId)
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
            return Redirect("/Home/Images");
            //  return RedirectToAction("Edit", new { id = employee.Id,name=employee.FirstName});



        }



    }

}

