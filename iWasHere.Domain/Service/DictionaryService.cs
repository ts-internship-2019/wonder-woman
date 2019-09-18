using iWasHere.Domain.DTOs;
using iWasHere.Domain.Model;
using iWasHere.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace iWasHere.Domain.Service
{
    public class DictionaryService
    {
        private readonly DatabaseContext _dbContext;
        public DictionaryService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public List<DictionaryLandmarkTypeModel> GetDictionaryLandmarkTypeModels()
        {
            List<DictionaryLandmarkTypeModel> dictionaryLandmarkTypeModels = _dbContext.DictionaryLandmarkType.Select(a => new DictionaryLandmarkTypeModel()
            {
                Id = a.LandmarkTypeId,
                Name = a.Name
            }).ToList();
            return dictionaryLandmarkTypeModels;
        }


        //ewifhfew
        public List<DictionaryCurrencyType> GetDictionaryCurrencyTypeModels(int page, int pageSize, out int count)
        {
            int skip = (page - 1) * pageSize;

            List<DictionaryCurrencyType> dictionaryCurrencyTypes = _dbContext.DictionaryCurrencyType.Select(a => new DictionaryCurrencyType
            {
                CurrencyTypeId = a.CurrencyTypeId,
                Name = a.Name,
                Code = a.Code,
                Description = a.Description,
                CurrencyCountryId = a.CurrencyCountryId,
                CurrencyCountry = a.CurrencyCountry
            }).Skip(skip).Take(pageSize).ToList();

            count = _dbContext.DictionaryCurrencyType.Count();

            return dictionaryCurrencyTypes;
        }
    }
}
