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



        public List<DictionaryTicketTypeModel> GetDictionaryTicketTypeModels(int currentPage, int pageSize, out int count)
        {
            int rowsToSkip = (currentPage - 1) * pageSize;
            count = Convert.ToInt32(_dbContext.DictionaryTicketType.Count());

            List<DictionaryTicketTypeModel> dictionaryTicketTypeModels = _dbContext.DictionaryTicketType.Select(a => new DictionaryTicketTypeModel()
            {
                TicketTypeId = a.TicketTypeId,
                Code = a.Code,
                Name = a.Name,
                Description = a.Description
            }).Skip(rowsToSkip).Take(pageSize).ToList();
            return dictionaryTicketTypeModels;
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

        public List<DictionaryCurrencyType> GetFilteredDictionaryCurrencyTypeModels(int page, int pageSize, string name, out int count)
        {
            List<DictionaryCurrencyType> dictionaryCurrencyTypes = _dbContext.DictionaryCurrencyType.Select(a => new DictionaryCurrencyType
            {
                CurrencyTypeId = a.CurrencyTypeId,
                Name = a.Name,
                Code = a.Code,
                Description = a.Description,
                CurrencyCountryId = a.CurrencyCountryId,
                CurrencyCountry = a.CurrencyCountry
            }).Where(a => a.Name.Contains(name)).ToList();
            count = dictionaryCurrencyTypes.Count();
            int skip = (page - 1) * pageSize;
            return dictionaryCurrencyTypes.Skip(skip).Take(pageSize).ToList(); ;
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
                CountyId = c.CountyId,
                Name = c.Name
            });
            return query.ToList();
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

        //filtrare Country
        public List<DictionaryCountryModel> GetFilteredCountryModels(int page, int pageSize, out int count, string filterName)
        {
            int skip = (page - 1) * pageSize;
            //count = _dbContext.Country.Count();

            List<DictionaryCountryModel> country = _dbContext.Country.Select(a => new DictionaryCountryModel()
            {
                CountryId = a.CountryId,
                Name = a.Name,
                Code = a.Code,
                ParentId = a.ParentId
            }).Where(a=>a.Name.Contains(filterName)).ToList();
            count = country.Count();
            return country.Skip(skip).Take(pageSize).ToList();
        }



        public List<CountyModel> GetCountyModels(int page,int pageSize,out int count)
        {
            int skip = (page - 1) * pageSize;
            count = _dbContext.County.Count();
            List<CountyModel> listCounties = _dbContext.County.Include(a=>a.Country).Select(a => new CountyModel()
            {
                CountyId = a.CountyId,
                Name = a.Name,
                Code = a.Code,
                CountryId = a.CountryId,
                CountryName=a.Country.Name
            }).Skip(skip).Take(pageSize).ToList();

            return listCounties ;
        }

        public List<DictionaryConstructionTypeModel> GetDictionaryConstructionTypeModels(int currentPage,int pageSize, out int count)
        {
            int rowsToSkip = (currentPage - 1) * pageSize;
            count = Convert.ToInt32(_dbContext.DictionaryConstructionType.Count());
            List<DictionaryConstructionTypeModel> dictionaryConstructionTypeModels = _dbContext.DictionaryConstructionType.Select(a => new DictionaryConstructionTypeModel()
            {
                ConstructionTypeId = a.ConstructionTypeId,
                Code = a.Code,
                Name = a.Name,
                Description = a.Description
            }).Skip(rowsToSkip).Take(pageSize).ToList();

            return dictionaryConstructionTypeModels;
        }
    }
}
