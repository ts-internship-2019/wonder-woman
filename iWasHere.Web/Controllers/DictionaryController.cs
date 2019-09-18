using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iWasHere.Domain.DTOs;
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
    }
}