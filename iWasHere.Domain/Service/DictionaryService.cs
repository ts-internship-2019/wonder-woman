using iWasHere.Domain.DTOs;
using iWasHere.Domain.Model;
using iWasHere.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;


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
            return dictionaryLandmarkTypeModels;
        } 
        public List<CityModel> GetAllPagedCities(int skipRows, int pageSize, string filterName, int filterCounty, out int totalRows)
        {
            totalRows = 0;
            if (filterCounty > 0)
            {
                var query = _dbContext.City.Where(a => a.Name.Contains(filterName)).Include(b => b.County).Where(b => b.CountyId.Equals(filterCounty));
                if (query.Count() > 0)
                {
                    var page = query.OrderBy(p => p.CityId)
                                .Select(p => new CityModel()
                                {
                                    Id = p.CityId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    CountyId = p.CountyId,
                                    CountyName = p.County.Name
                                })
                                .Skip(skipRows).Take(pageSize)
                                .GroupBy(p => new { Total = query.Count() })
                                .First();
                    totalRows = page.Key.Total;
                    var cities = page.Select(p => p);
                    return cities.ToList();
                }
            }
            else
            {
                var query = _dbContext.City.Where(a => a.Name.Contains(filterName)).Include(b => b.County);
                if (query.Count() > 0)
                {
                    var page = query.OrderBy(p => p.CityId)
                                .Select(p => new CityModel()
                                {
                                    Id = p.CityId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    CountyId = p.CountyId,
                                    CountyName = p.County.Name
                                })
                                .Skip(skipRows).Take(pageSize)
                                .GroupBy(p => new { Total = query.Count() })
                                .First();
                    totalRows = page.Key.Total;
                    var cities = page.Select(p => p);
                    return cities.ToList();
                }
            }
            
            return new List<CityModel>();            
        }
        public List<CountyModel> GetCounties()
        {
            var query = _dbContext.County.Select(c => new CountyModel()
            {
                Id = c.CountyId,
                Name = c.Name
            });
            return query.ToList();
        }
    }
}
