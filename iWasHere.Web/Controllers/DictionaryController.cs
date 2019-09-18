using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using iWasHere.Domain.Service;
using Kendo.Mvc.Extensions;
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

        public IActionResult IndexCountry()
        {
            return View();
        }


        //paginare

        [HttpPost]
        public ActionResult Paging_Orders([DataSourceRequest] DataSourceRequest request)
        {
            List<DictionaryCountryModel> countryModels = _dictionaryService.GetCountryModels(request.Page, request.PageSize, out int count).ToList();
            DataSourceResult result = new DataSourceResult()
            {
                Data = countryModels,
                Total = count
            };
            return Json(result);
        }

        public IActionResult AddNewCountry()
        {
            return View();
        }
    }
}