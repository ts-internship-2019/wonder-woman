using iWasHere.Domain.DTOs;
using iWasHere.Domain.Model;
using iWasHere.Domain.Models;
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
                Name = a.Name,
                Code = a.Code,
                Description = a.Description
            }).ToList();

            return dictionaryLandmarkTypeModels;
        }

        public List<DictionaryCountryModel> GetCountryModels(int page, int pageSize, out int count)
        {
            int skip = (page - 1) * pageSize;
            count = _dbContext.Country.Count();

            List<DictionaryCountryModel> country = _dbContext.Country.Select(a => new DictionaryCountryModel()
            {
                CountryId = a.CountryId,
                Name = a.Name,
                Code = a.Code,
                ParentId = a.ParentId
            }).Skip(skip).Take(pageSize).ToList();

            return country;
        }
    }
}
