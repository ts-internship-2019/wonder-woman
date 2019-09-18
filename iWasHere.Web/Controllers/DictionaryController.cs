using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using iWasHere.Domain.Service;
using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

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
            return View();
        }

        public IActionResult Tickets([DataSourceRequest] DataSourceRequest request)
        {
            return View();
        }

        public ActionResult GetAllTickets([DataSourceRequest] DataSourceRequest request)
        {
            List<DictionaryTicketTypeModel> dictionaryTicketTypeModels = _dictionaryService.GetDictionaryTicketTypeModels(request.Page, request.PageSize, out int count);

            DataSourceResult result = new DataSourceResult();
            result.Data = dictionaryTicketTypeModels;
            result.Total = count;
            return Json(result);
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

        public IActionResult Currency()
        {
            return View();
        }

        public IActionResult Landmark()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CurrencyRead([DataSourceRequest]DataSourceRequest request, string filterName)
        {
            DataSourceResult result = new DataSourceResult();
            if (string.IsNullOrWhiteSpace(filterName))
            {
                result.Data = _dictionaryService.GetLandmarkTypeModels(request.Page, request.PageSize, out int count);
                result.Total = count;
            }
            else
            {
                result.Data = _dictionaryService.GetFilteredLandmarkTypeModels(request.Page, request.PageSize, filterName, out int count);
                result.Total = count;
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult LandmarkTypeRead([DataSourceRequest]DataSourceRequest request, string filterName)
        {
            DataSourceResult result = new DataSourceResult();
            if (string.IsNullOrWhiteSpace(filterName))
            {
                result.Data = _dictionaryService.GetLandmarkTypeModels(request.Page, request.PageSize, out int count);
                result.Total = count;
            }
            else
            {
                result.Data = _dictionaryService.GetFilteredLandmarkTypeModels(request.Page, request.PageSize, filterName, out int count);
                result.Total = count;
            }
            return Json(result);
        }

        public IActionResult IndexCountry()
        {
            return View();
        }


        //paginare

        [HttpPost]
        public ActionResult Paging_Orders_Country([DataSourceRequest] DataSourceRequest request, string filterName)
        {
            List<DictionaryCountryModel> countryModels = new List<DictionaryCountryModel>();
            DataSourceResult result = new DataSourceResult();
            if (string.IsNullOrWhiteSpace(filterName))
            {
                countryModels = _dictionaryService.GetCountryModels(request.Page, request.PageSize, out int count).ToList();
                result.Data = countryModels;
                result.Total = count;
            }
            else
            {
                countryModels = _dictionaryService.GetFilteredCountryModels(request.Page, request.PageSize, out int count, filterName).ToList();
                result.Data = countryModels;
                result.Total = count;
            }
            return Json(result);
        }

        public IActionResult AddNewCountry()
        {
            return View();
        }

        public IActionResult CurrencyAdd()
        {
            return View();
        }

        public IActionResult LandmarkAdd()
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

        public IActionResult Counties_Read([DataSourceRequest] DataSourceRequest request, string filterName)
        {
            if (String.IsNullOrEmpty(filterName))
            {
                filterName = "";
            }
            int filterCountry = 1;
            DataSourceResult result = new DataSourceResult();
            List<CountyModel> list = GetCounties(request.Page, request.PageSize, filterName, filterCountry, out int totalRows);
            result.Data = list;
            result.Total = totalRows;
            return Json(result);
        }

        private List<CountyModel> GetCounties(int page, int pageSize, string filterName, int filterCountry, out int totalRows)
        {
            int skipRows = (page - 1) * pageSize;
            List<CountyModel> countyModels = _dictionaryService.GetAllPagedCounties(skipRows, pageSize, filterName, filterCountry, out int rowsCount);
            totalRows = rowsCount;
            return countyModels;
        }

        public IActionResult Countries_Read_ForCB([DataSourceRequest] DataSourceRequest request)
        {
            //DataSourceResult result = new DataSourceResult();
            //List<CountyModel> list = GetCountiesForCB();
            //result.Data = list;
            DataSourceResult result = GetCountriesForCB().ToDataSourceResult(request);
            return Json(result);
        }

        public List<DictionaryCountryModel> GetCountriesForCB()
        {
            List<DictionaryCountryModel> countryModels = _dictionaryService.GetCountries();
            return countryModels;
        }


        public IActionResult AddNewCounty()
        {
            return View();
        }

    
        public IActionResult Construction([DataSourceRequest] DataSourceRequest request)
        {
            return View();
        }

        public ActionResult GetConstruction([DataSourceRequest] DataSourceRequest request, string filterName)
        {
            List<DictionaryConstructionTypeModel> dictionaryConstructionType = _dictionaryService.GetDictionaryConstructionTypeModels(filterName, request.Page, request.PageSize, out int count);
            DataSourceResult result = new DataSourceResult();
            result.Data = dictionaryConstructionType;
            result.Total = count;
            return Json(result);
        }

    }
}