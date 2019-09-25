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
using System.Net.Mail;
using System.Net;

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
            ViewData["Country"] = _homeService.GetCountryByLandmarkId(id);
            return View(model);
        }

        public ActionResult GetLandmarks([DataSourceRequest] DataSourceRequest request, int? filterId)
        {
            List<LandmarkModel> landmarkList = new List<LandmarkModel>();
            if (filterId == null || filterId < 1)
            {
                landmarkList = _homeService.GetLandmarkListModels();
            }
            else
            {
                landmarkList = _homeService.GetLandmarksByCountryId(filterId);
            }

            DataSourceResult result = new DataSourceResult();
            result.Data = landmarkList;
            return Json(result);
        }
            public IActionResult Landmarks_List_Read([DataSourceRequest] DataSourceRequest request, int id)
        {
            ViewData["CountryId"] = id;
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

        public ActionResult DestroyLandmark([DataSourceRequest] DataSourceRequest request, LandmarkModel landmarktoDestroy)
        {

            _homeService.DestroyLandmark(landmarktoDestroy,out string errorMessage);
            //TODO: HANDLE THE ERROR
            return Json(request);
        }


        public IActionResult ExportWord(int id)

        {

            LandmarkModel model = _homeService.GetLandmarkById(id);

            Stream stream = _homeService.ExportToWord(model);

            return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Landmark.docx");

        }

        [HttpPost]
        public IActionResult CommentSubmit(LandmarkModel ldm)
        {
            Comment comm = new Comment()
            {
                LandmarkId = ldm.LandmarkId,
                Text = ldm.CommentText,
                Title = ldm.CommentTitle,
                OwnerName = ldm.CommentOwner,
                RatingValue = ldm.RatingValue
            };
            _homeService.AddComments(comm, out string errorMessage2);
            if (!string.IsNullOrEmpty(errorMessage2))
            {
                TempData["message"] = errorMessage2;
            }
            return RedirectToAction("Landmark_Read", new { id = comm.LandmarkId });
        }

        public IActionResult SendEmail([DataSourceRequest] DataSourceRequest request, String email, int id)

        {

            bool sent = false;


            LandmarkModel model = _homeService.GetLandmarkById(id);
            var fromAddress = new MailAddress("scarlterwitch@gmail.com", "From WonderWoman");

            var toAddress = new MailAddress("georgiana.udrea95.gu@gmail.com", "To Name");

            const string fromPassword = "ThisIsNotAPassword123";

            const string body = "We've attached the landmark in this email!";

            

            // MemoryStream ms = _dictionaryService.ExportFileAlice(id);

            // Attachment data = new Attachment(ms, "Landmark.docx", System.Net.Mime.MediaTypeNames.Text.Plain);

            Attachment data = new Attachment(_homeService.ExportToWord(model), "Landmark.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            var smtp = new SmtpClient

            {

                Host = "smtp.gmail.com",

                Port = 587,

                EnableSsl = true,

                DeliveryMethod = SmtpDeliveryMethod.Network,

                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)

            };

            MailMessage message = new MailMessage(fromAddress, toAddress);

            message.Subject = "Landmark Attachment";

            message.Body = body;

            message.Attachments.Add(data);

            {

                try

                {

                    smtp.Send(message);

                    sent = true;

                }
                catch (Exception ex)

                {

                    Console.WriteLine(ex.Message);

                    sent = false;

                }

            }

            return  RedirectToAction("Landmarks_List_Read", new { id = model.LandmarkId });

        }
    }
}

