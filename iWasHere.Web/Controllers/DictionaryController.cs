using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using iWasHere.Domain.Models;
using Kendo.Mvc.Extensions;
using FluentNHibernate.Conventions.Inspections;
using System.Web.Helpers;

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
    }
}