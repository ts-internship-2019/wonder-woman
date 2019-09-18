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
        /// <summary>
        /// IActionResult for Cities that populpates the Kendo UI Grid in the IndexCity View
        /// </summary>
        /// <param name="request"></param>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public  IActionResult Cities_Read([DataSourceRequest] DataSourceRequest request, string filterName, int filterCounty)
        {            
            if (String.IsNullOrEmpty(filterName))
            {
                filterName = "";
            }            
            DataSourceResult result = new DataSourceResult();            
            List<CityModel> list = GetCities(request.Page, request.PageSize, filterName, filterCounty, out int totalRows);
            result.Data = list;
            result.Total = totalRows;
            return Json(result);
        }
        /// <summary>
        /// Test IActionResult for Counties ComboBox
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult Couties_Read_ForCB(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                text = "";
            }
            List<CountyModel> result = GetCountiesForCB(text);            
            return Json(result);
        }
        /// <summary>
        /// Test Gets Counties as a List<>
        /// </summary>
        /// <returns></returns>
        public List<CountyModel> GetCountiesForCB(string filterCounty)
        {
            List<CountyModel> countyModels = _dictionaryService.GetCounties(filterCounty);
            return countyModels;
        }
        /// <summary>
        /// Test Gets Cities as a List<>
        /// </summary>
        /// <returns></returns>
        private List<CityModel> GetCities(int page, int pageSize, string filterName, int filterCounty, out int totalRows)
        {            
            int skipRows = (page - 1) * pageSize;
            List<CityModel> cityModels = _dictionaryService.GetAllPagedCities(skipRows, pageSize, filterName, filterCounty, out int rowsCount);
            totalRows = rowsCount;
            return cityModels;
        }
        /// <summary>
        /// NOT IMplemeted Yet
        /// </summary>
        /// <returns></returns>
        public IActionResult AddCity()
        {
            return View();
        }
    }
}