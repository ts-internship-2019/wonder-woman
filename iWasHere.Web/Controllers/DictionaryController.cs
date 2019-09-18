using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using iWasHere.Domain.Service;
using Microsoft.AspNetCore.Mvc;

using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;

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

        public IActionResult Currency()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CurrencyRead([DataSourceRequest]DataSourceRequest request)
        {
            DataSourceResult result = new DataSourceResult()
            {
                Data = _dictionaryService.GetDictionaryCurrencyTypeModels(request.Page, request.PageSize, out int count),
                Total = count
            };

            return Json(result);
        }

        [HttpPost]
        public ActionResult Filter(DictionaryCurrencyType model)
        {
            if (ModelState.IsValid)
            {
               
            }

            return View("Currency", model);
        }
    }
}