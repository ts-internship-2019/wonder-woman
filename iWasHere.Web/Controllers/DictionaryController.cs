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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using iWasHere.Web.Models;
using Microsoft.AspNetCore.Routing;

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

        public ActionResult GetAllTickets([DataSourceRequest] DataSourceRequest request, string filterName)
        {
            List<DictionaryTicketTypeModel> dictionaryTicketTypeModels = _dictionaryService.GetDictionaryTicketTypeModels(filterName, request.Page, request.PageSize, out int count);

            DataSourceResult result = new DataSourceResult();
            result.Data = dictionaryTicketTypeModels;
            result.Total = count;
            return Json(result);
        }
        public ActionResult DestroyTicket([DataSourceRequest] DataSourceRequest request, DictionaryTicketTypeModel ticketToDelete)
        {
            _dictionaryService.DestroyTicket(ticketToDelete);
            return Json(request);
        }
        public IActionResult AddTicket(int Id)
        {
            DictionaryTicketTypeModel ticket = new DictionaryTicketTypeModel();
            if (Id != 0)
                ticket = _dictionaryService.GetTicketById(Id);


            return View(ticket);
        }
        [HttpPost]
        public ActionResult UpdateTicket(DictionaryTicketTypeModel ticketToUpdate, string submit)
        {
            switch (submit)
            {
                case "Salveaza si nou":
                    _dictionaryService.UpdateTicket(ticketToUpdate);
                    return Redirect("/Dictionary/AddTicket");
                case "Salveaza":
                    _dictionaryService.UpdateTicket(ticketToUpdate);
                    return Redirect("/Dictionary/Tickets");
                case "Anuleaza":
                    return Redirect("/Dictionary/Tickets");
                default:
                    return Redirect("/Dictionary/Tickets");
            }
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
        public IActionResult Cities_Read([DataSourceRequest] DataSourceRequest request, string filterName, int filterCounty)
        {
            DataSourceResult result = new DataSourceResult();
            if (string.IsNullOrWhiteSpace(filterName))
            {
                if (filterCounty == 0)
                {
                    result.Data = _dictionaryService.GetAllPagedCities(request.Page, request.PageSize, out int count);
                    result.Total = count;
                }
                else
                {
                    result.Data = _dictionaryService.GetFilteredOnlyByCountyPagedCities(request.Page, request.PageSize, filterCounty, out int count);
                    result.Total = count;
                }
                
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(filterName) && filterCounty > 0)
                {
                    result.Data = _dictionaryService.GetFilteredPagedCities(request.Page, request.PageSize, filterName, filterCounty, out int count);
                    result.Total = count;
                }
                else if(!string.IsNullOrWhiteSpace(filterName) && filterCounty == 0)
                {
                    result.Data = _dictionaryService.GetFilteredOnlyByNamePagedCities(request.Page, request.PageSize, filterName, out int count);
                    result.Total = count;
                }
                
            }
            return Json(result);
        }
        /// <summary>
        /// Test IActionResult for Counties ComboBox
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public JsonResult Couties_Read_ForCB(string text)
        {            
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
        /// Adds a new city
        /// </summary>
        /// <returns></returns>
        public IActionResult AddCity(int id, string name = "", string code = "", int countyId = 0, string countyName = "")
        {
            CityModel city = new CityModel();
            if (!string.IsNullOrEmpty(name))
            {
                city.Id = id;
                city.Name = name;
                city.Code = code;
                city.CountyId = countyId;
                city.CountyName = countyName;
            }
            else
            {
                city = _dictionaryService.GetCityInfoById(id);
            }            
            return View(city);
        }            

        public ActionResult SaveCity(CityModel city, string btn)
        {
            switch (btn)
            {
                case "Salveaza si Nou":
                    _dictionaryService.SaveCity(city, out string errorMessage);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData["message"] = errorMessage;
                        

                        return RedirectToAction("AddCity", new RouteValueDictionary(city));
                    }
                    return Redirect("/Dictionary/AddCity");
                case "Salveaza":
                    _dictionaryService.SaveCity(city,out string errorMessage2);
                    if (!string.IsNullOrEmpty(errorMessage2))
                    {
                        TempData["message"] = errorMessage2;
                        

                        return RedirectToAction("AddCity", new RouteValueDictionary(city));
                    }
                    return Redirect("/Dictionary/IndexCity");
                case "Anuleaza":                    
                    return Redirect("/Dictionary/IndexCity");
                default:                    
                    return Redirect("/Dictionary/IndexCity");
            }                  
            
        }
        /// <summary>
        /// Destroys the selected city
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cityToDestroy"></param>
        /// <returns></returns>
        public IActionResult DestroyCity([DataSourceRequest] DataSourceRequest request, CityModel cityToDestroy)
        {
            _dictionaryService.DestroyCity(cityToDestroy, out string errorMessage);

            if (string.IsNullOrEmpty(errorMessage))
            {
                return Json(request);
            }
            else
            {
                return Json(new DataSourceResult
                {
                    Errors = errorMessage
                });
            }         

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
        public ActionResult Index(DictionaryCurrencyType model)
        {
            int currencyTypeId = model.CurrencyTypeId;
            string name = model.Name;
            string code = model.Code;
            string description = model.Description;

            return View();
        }

        [HttpPost]
        public ActionResult CurrencyRead([DataSourceRequest]DataSourceRequest request, string filterName)
        {
            DataSourceResult result = new DataSourceResult();
            if (string.IsNullOrWhiteSpace(filterName))
            {
                result.Data = _dictionaryService.GetDictionaryCurrencyTypeModels(request.Page, request.PageSize, out int count);
                result.Total = count;
            }
            else
            {
                result.Data = _dictionaryService.GetFilteredDictionaryCurrencyTypeModels(request.Page, request.PageSize, filterName, out int count);
                result.Total = count;
            }
            return Json(result);
        }   

        [HttpPost]
        public ActionResult CurrencyDestroy([DataSourceRequest] DataSourceRequest request, iWasHere.Domain.Models.DictionaryCurrencyType currency)
        {
            string error;
            if (currency != null && ModelState.IsValid)
            {
                error = _dictionaryService.CurrencyDelete(currency.CurrencyTypeId);
                if(!string.IsNullOrWhiteSpace(error))
                    ModelState.AddModelError("a", error);
            }
            return Json(ModelState.ToDataSourceResult());
        }

        [HttpPost]
        public ActionResult LandmarkDestroy([DataSourceRequest] DataSourceRequest request, iWasHere.Domain.Models.DictionaryLandmarkType landmark)
        {
            if (landmark != null && ModelState.IsValid)
            {
                _dictionaryService.LandmarkDelete(landmark.LandmarkTypeId);
            }

            return Json(ModelState.ToDataSourceResult());
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


        //textbox
        //updatebutton
        [HttpPost]
        public IActionResult CountrySubmit(Country model, string btnSave)
        {
            switch (btnSave)
            {
                case "Save":
                    _dictionaryService.UpdateCountry(model, out string errorMessage2);
                    if(!string.IsNullOrEmpty(errorMessage2))
                    {
                        TempData["message"] = errorMessage2;
                        return RedirectToAction("AddNewCountry", new { id = model.CountryId });
                    }
                    return Redirect("/Dictionary/IndexCountry");
                case "Save and New":
                    _dictionaryService.UpdateCountry(model, out string errorMessage);
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        TempData["message"] = errorMessage;
                        return RedirectToAction("AddNewCountry", new { id = model.CountryId });
                    }
                    return Redirect("/Dictionary/IndexCountry");
                default:
                    return Redirect("/Dictionary/IndexCountry");
            }
        }

        //paginare tari

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

        //delete Country
        public ActionResult DestroyCountry([DataSourceRequest] DataSourceRequest request, DictionaryCountryModel countryToDelete)
        {
            _dictionaryService.DestroyCountry(countryToDelete);
            return Json(request);
        }

        public IActionResult AddNewCountry(int id)
        {
            if(id == 0)
            {
                return View();
            }
            else
            {
                return View(_dictionaryService.editFunctionForCountry(id));
            }
        }

        public IActionResult CurrencyAdd(int id)
        {   
            if (id == 0)
            {
                return View();
            }
            else
            {
                DictionaryCurrencyType model = new DictionaryCurrencyType();
                model = _dictionaryService.GetCurrencyModel(id);
                return View(model);
            }
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

        public JsonResult Countries_Read_ForCB(string text)
        {            
            List<DictionaryCountryModel> list = GetCountriesForCB(text);          
            return Json(list);
        }

        public List<DictionaryCountryModel> GetCountriesForCB(string text)
        {
            List<DictionaryCountryModel> countryModels = _dictionaryService.GetCountries(text);
            return countryModels;
        }


        public IActionResult AddNewCounty(int id)
        {
            if (id == 0)
                return View();
            else
 
                return View(_dictionaryService.editCounty(id));
                
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
        public ActionResult DestroyConstruction([DataSourceRequest] DataSourceRequest request, DictionaryConstructionTypeModel constructionToDelete)
        {
            _dictionaryService.DestroyConstruction(constructionToDelete);
            //return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            return Json(request);
        }

        [HttpPost]
        public ActionResult CurrencySubmit(DictionaryCurrencyType model, string submitButton)
        {
            switch(submitButton)
            {
                case "Save":
                    if (_dictionaryService.CurrencyUpdateInsert(model))
                        return Redirect("/Dictionary/Currency");
                    else
                        return RedirectToAction("CurrencyAdd", new { id = model.CurrencyTypeId });


                case "Save and New":
                    if (_dictionaryService.CurrencyUpdateInsert(model))
                        return Redirect("/Dictionary/CurrencyAdd");
                    else
                        return RedirectToAction("CurrencyAdd", new { id = model.CurrencyTypeId });

                default:
                    return Redirect("/Dictionary/Currency");
            }
        }
        public ActionResult DestroyCounty([DataSourceRequest] DataSourceRequest request, CountyModel countyToDelete)
        {
            _dictionaryService.DestroyCounty(countyToDelete);
            //return Json(new[] { product }.ToDataSourceResult(request, ModelState));
            return Json(request);
        }

    }
}