using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using iWasHere.Domain.Service;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using iWasHere.Domain.Models;
using Kendo.Mvc.Extensions;
using FluentNHibernate.Conventions.Inspections;
using System.Web.Helpers;

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

        public IActionResult IndexCity()
        {            
            return View();
        }

        public  IActionResult Cities_Read([DataSourceRequest] DataSourceRequest request, string filterName)
        {            
            if (String.IsNullOrEmpty(filterName))
            {
                filterName = "";
            }
            int filterCounty = 1;
            DataSourceResult result = new DataSourceResult();            
            List<CityModel> list = GetCities(request.Page, request.PageSize, filterName, filterCounty, out int totalRows);
            result.Data = list;
            result.Total = totalRows;
            return Json(result);
        }

        public IActionResult Couties_Read_ForCB([DataSourceRequest] DataSourceRequest request)
        {
            //DataSourceResult result = new DataSourceResult();
            //List<CountyModel> list = GetCountiesForCB();
            //result.Data = list;
            DataSourceResult result = GetCountiesForCB().ToDataSourceResult(request);
            return Json(result);
        }

        public List<CountyModel> GetCountiesForCB()
        {
            List<CountyModel> countyModels = _dictionaryService.GetCounties();
            return countyModels;
        }

        private List<CityModel> GetCities(int page, int pageSize, string filterName, int filterCounty, out int totalRows)
        {            
            int skipRows = (page - 1) * pageSize;
            List<CityModel> cityModels = _dictionaryService.GetAllPagedCities(skipRows, pageSize, filterName, filterCounty, out int rowsCount);
            totalRows = rowsCount;
            return cityModels;
        }

        public IActionResult AddCity()
        {
            return View();
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

        public IActionResult IndexCountry()
        {
            return View();
        }


        //paginare

        [HttpPost]
        public ActionResult Paging_Orders_Country([DataSourceRequest] DataSourceRequest request)
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

        public IActionResult IndexCounty()
        {
          
            return View();
        }
        public ActionResult Paging_Orders_County([DataSourceRequest] DataSourceRequest request)
        {
            List<CountyModel> list = _dictionaryService.GetCountyModels(request.Page, request.PageSize, out int count).ToList();
            DataSourceResult result = new DataSourceResult()
            {
                Data = list,
                Total = count
            };
            return Json(result);
        }

       public IActionResult AddNewCounty()
        {
            return View();
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