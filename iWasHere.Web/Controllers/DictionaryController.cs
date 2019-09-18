using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using iWasHere.Domain.Service;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;

namespace iWasHere.Web.Controllers
{
    public class DictionaryController : Controller
    {
        private readonly DictionaryService _dictionaryService;

        public DictionaryController(DictionaryService dictionaryService)
        {
            _dictionaryService = dictionaryService;
        }

        public IActionResult Index()
        {
            List<DictionaryLandmarkTypeModel> dictionaryLandmarkTypeModels = _dictionaryService.GetDictionaryLandmarkTypeModels();

            return View(dictionaryLandmarkTypeModels);
        }
        public IActionResult Construction([DataSourceRequest] DataSourceRequest request)
        {

           
            return View();
        }

        public ActionResult GetConstruction([DataSourceRequest] DataSourceRequest request)
        {
            List<DictionaryConstructionTypeModel> dictionaryConstructionType = _dictionaryService.GetDictionaryConstructionTypeModels(request.Page, request.PageSize, out int count);
            DataSourceResult result = new DataSourceResult();
            result.Data = dictionaryConstructionType;
            result.Total = count;
            return Json(result);
        }

    }
}