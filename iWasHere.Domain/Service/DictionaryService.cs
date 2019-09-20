using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace iWasHere.Domain.Service
{
    public class DictionaryService
    {
        private readonly DatabaseContext _dbContext;
        public DictionaryService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public void CurrencyUpdateInsert(DictionaryCurrencyType model)
        {
            if (model.CurrencyTypeId == 0)
            {
                _dbContext.DictionaryCurrencyType.Add(model);
                _dbContext.SaveChanges();
            }
            else
            {
                _dbContext.DictionaryCurrencyType.Update(model);
                _dbContext.SaveChanges();
            }
        }
        public List<DictionaryTicketTypeModel> GetDictionaryTicketTypeModels(string filterName, int currentPage, int pageSize, out int count)
        {
            int rowsToSkip = (currentPage - 1) * pageSize;

            if (!String.IsNullOrWhiteSpace(filterName))
            {
                var query = _dbContext.DictionaryTicketType.Where(a => a.Name.Contains(filterName));
                if (query.Count() > 0)
                {
                    var page = query.OrderBy(p => p.TicketTypeId)
                                .Select(p => new DictionaryTicketTypeModel()
                                {
                                    TicketTypeId = p.TicketTypeId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    Description = p.Description
                                })
                                .Skip(rowsToSkip).Take(pageSize)
                                .GroupBy(p => new { Total = query.Count() })
                                .First();
                    count = page.Key.Total;
                    var tickets = page.Select(p => p);
                    return tickets.ToList();
                }
            }
            else
            {
                 var page = _dbContext.DictionaryTicketType.OrderBy(p => p.TicketTypeId)
                                .Select(p => new DictionaryTicketTypeModel()
                                {
                                    TicketTypeId = p.TicketTypeId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    Description = p.Description
                                })
                                .Skip(rowsToSkip).Take(pageSize)
                                .GroupBy(p => new { Total = _dbContext.DictionaryTicketType.Count() })
                                .First();
                    count = page.Key.Total;
                    var tickets = page.Select(p => p);
                    return tickets.ToList();
                
            }
            count = 0;
            return new List<DictionaryTicketTypeModel>();

        }
        public void DestroyTicket(DictionaryTicketTypeModel ticketToDestroy)
        {
            var db = _dbContext;

            var tickets = db.DictionaryTicketType.Where(pd => pd.TicketTypeId == ticketToDestroy.TicketTypeId);

            foreach (var tckt in tickets)
            {
                db.DictionaryTicketType.Remove(tckt);
            }

            db.SaveChanges();
        }

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

        public void CurrencyDelete(int id)
        {
            DictionaryCurrencyType deleted = _dbContext.DictionaryCurrencyType.First(a => a.CurrencyTypeId == id);
            _dbContext.DictionaryCurrencyType.Remove(deleted);
            try
            {
                _dbContext.SaveChanges();
            }
            catch(Exception ex)
            {
            }
        }

        public void LandmarkDelete(int id)
        {
            DictionaryLandmarkType deleted = _dbContext.DictionaryLandmarkType.First(a => a.LandmarkTypeId == id);
            _dbContext.DictionaryLandmarkType.Remove(deleted);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
            }
        }

        public DictionaryCurrencyType GetCurrencyModel(int id)
        {
            return _dbContext.DictionaryCurrencyType.First(a => a.CurrencyTypeId == id);
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

        public void SaveCity(CityModel city)
        {
            City cityToSave = new City();
            cityToSave.CityId = city.Id;
            cityToSave.Name = city.Name;
            cityToSave.Code = city.Code;
            cityToSave.CountyId = city.CountyId;
            if (city.Id == 0)
            {                
                _dbContext.City.Add(cityToSave);
            }
            else
            {                
                _dbContext.City.Update(cityToSave);
            }
            _dbContext.SaveChanges();

        }

        public CityModel GetCityInfoById(int id)
        {
            CityModel city = new CityModel();
            List<CityModel> cities = new List<CityModel>(); 
            var query = _dbContext.City.Where(a => a.CityId.Equals(id)).Include(b => b.County);
            if (query.Count() == 1)
            {
                var page = query.Select(a => new CityModel
                {
                    Id = a.CityId,
                    Name = a.Name,
                    Code = a.Code,
                    CountyId = a.County.CountyId,
                    CountyName = a.County.Name
                });
                cities = page.ToList();
            }
            if(cities.Count == 1)
            {
                city = cities[0];
            }
            return city;
        }

        public List<DictionaryLandmarkType> GetLandmarkTypeModels(int page, int pageSize, out int count)
        {
            int skip = (page - 1) * pageSize;
            List<DictionaryLandmarkType> dictionaryLandmarkTypes = _dbContext.DictionaryLandmarkType.Select(a => new iWasHere.Domain.Models.DictionaryLandmarkType
            {
                LandmarkTypeId = a.LandmarkTypeId,
                Name = a.Name,
                Code = a.Code,
                Description = a.Description,
            }).Skip(skip).Take(pageSize).ToList();
            count = _dbContext.DictionaryLandmarkType.Count();
            return dictionaryLandmarkTypes;
        }

        public List<DictionaryLandmarkType> GetFilteredLandmarkTypeModels(int page, int pageSize, string name, out int count)
        {
            List<DictionaryLandmarkType> dictionaryLandmarkTypes = _dbContext.DictionaryLandmarkType.Select(a => new iWasHere.Domain.Models.DictionaryLandmarkType
            {
                LandmarkTypeId = a.LandmarkTypeId,
                Name = a.Name,
                Code = a.Code,
                Description = a.Description
            }).Where(a => a.Name.Contains(name)).ToList();
            count = dictionaryLandmarkTypes.Count();
            int skip = (page - 1) * pageSize;
            return dictionaryLandmarkTypes.Skip(skip).Take(pageSize).ToList(); ;
        }           
        /// <summary>
        /// Destroys a city
        /// </summary>
        /// <param name="cityToDestroy"></param>
        public void DestroyCity(CityModel cityToDestroy)
        {
            var db = _dbContext;
            var cities = db.City.Where(pd => pd.CityId == cityToDestroy.Id);
            foreach (var city in cities)
            {
                db.City.Remove(city);
            }
            db.SaveChanges();
        }
        

        /// <summary>
        /// Gets paged cities
        /// </summary>
        /// <param name="skipRows">Represents rows to skip to show the desired page</param>
        /// <param name="pageSize">Represents the # of rows to display per page</param>
        /// <param name="filterName">Presents the City Name to filter by</param>
        /// <param name="filterCounty">Represents the County ID to filter by</param>
        /// <param name="totalRows">Represents the total # of records in the DB mathincg the filtering criteria</param>
        /// <returns></returns>
        public List<CityModel> GetAllPagedCities(int skipRows, int pageSize, string filterName, int filterCounty, out int totalRows)
        {
            totalRows = 0;
            if (String.IsNullOrEmpty(filterName) && filterCounty == 0)
            {
                var query = _dbContext.City.Include(b => b.County);
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
                totalRows = query.Count();
                var cities = page.Select(p => p);
                return cities.ToList();
            }
            if (!String.IsNullOrEmpty(filterName))
            {
                if(filterCounty > 0)
                {
                    var query = _dbContext.City.Where(a => a.Name.Contains(filterName)).Include(b => b.County).Where(b => b.CountyId.Equals(filterCounty));
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
                    totalRows = query.Count();
                    var cities = page.Select(p => p);
                    return cities.ToList();
                }
                else
                {
                    var query = _dbContext.City.Where(a => a.Name.Contains(filterName)).Include(b => b.County);
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
                    totalRows = query.Count();
                    var cities = page.Select(p => p);
                    return cities.ToList();
                }
            }
            return new List<CityModel>();
        }
        /// <summary>
        /// Simple service method to get Counties for ComboBox
        /// </summary>
        /// <returns></returns>
        public List<CountyModel> GetCounties(string filterCounty)
        {
            var query = _dbContext.County.Select(c => new CountyModel()
            {
                CountyId = c.CountyId,
                Name = c.Name
            }).Where(c => c.Name.Contains(filterCounty));
            return query.ToList();
        }

        public List<DictionaryCountryModel> GetCountries(string text)
        {
            var query = _dbContext.Country.Select(c => new DictionaryCountryModel()
            {
                CountryId = c.CountryId,
                Name = c.Name
            }).Where(c=>c.Name.Contains(text));
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

        //delete button for Country
        public void DestroyCountry(DictionaryCountryModel countryToDestroy)
        {
            var db = _dbContext;
            var countries = db.Country.Where(pd => pd.CountryId == countryToDestroy.CountryId);
            foreach (var tckt in countries)
            {
                db.Country.Remove(tckt);
            }
            db.SaveChanges();
        }

        //edit with values
        public Country editFunctionForCountry(int id)
        {
            return _dbContext.Country.First(a => a.CountryId == id);
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

        //updatenewpage
        public string UpdateCountry(Country country)
        {
            if(country.CountryId == 0)
            {
                _dbContext.Country.Add(country);
            }
            else
            {
                _dbContext.Country.Update(country);
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        public List<CountyModel> GetAllPagedCounties(int skipRows, int pageSize, string filterName, int filterCountry, out int totalRows)
        {
            totalRows = 0;
            if (filterCountry > 0)
            {
                var query = _dbContext.County.Where(a => a.Name.Contains(filterName)).Include(b => b.Country).Where(b => b.CountryId.Equals(filterCountry));
                if (query.Count() > 0)
                {
                    var page = query.OrderBy(p => p.CountyId)
                                .Select(p => new CountyModel()
                                {
                                    CountyId = p.CountyId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    CountryId = p.CountryId,
                                    CountryName = p.Country.Name
                                })
                                .Skip(skipRows).Take(pageSize)
                                .GroupBy(p => new { Total = query.Count() })
                                .First();
                    totalRows = page.Key.Total;
                    var counties = page.Select(p => p);
                    return counties.ToList();
                }
            }
            else
            {
                var query = _dbContext.County.Where(a => a.Name.Contains(filterName)).Include(b => b.Country);
                if (query.Count() > 0)
                {
                    var page = query.OrderBy(p => p.CountyId)
                                .Select(p => new CountyModel()
                                {
                                    CountyId = p.CountyId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    CountryId = p.CountryId,
                                    CountryName = p.Country.Name
                                })
                                .Skip(skipRows).Take(pageSize)
                                .GroupBy(p => new { Total = query.Count() })
                                .First();
                    totalRows = page.Key.Total;
                    var counties = page.Select(p => p);
                    return counties.ToList();
                }
            }

            return new List<CountyModel>();
        }

        public List<DictionaryConstructionTypeModel> GetDictionaryConstructionTypeModels(string filterName, int currentPage, int pageSize, out int count)
        {
            int rowsToSkip = (currentPage - 1) * pageSize;
            if (!String.IsNullOrWhiteSpace(filterName))
            {
                var query = _dbContext.DictionaryConstructionType.Where(a => a.Name.Contains(filterName));
                if (query.Count() > 0)
                {
                    var page = query.OrderBy(p => p.ConstructionTypeId)
                                .Select(p => new DictionaryConstructionTypeModel()
                                {
                                    ConstructionTypeId = p.ConstructionTypeId,
                                    Name = p.Name,
                                    Code = p.Code,
                                    Description = p.Description
                                })
                                .Skip(rowsToSkip).Take(pageSize)
                                .GroupBy(p => new { Total = query.Count() })
                                .First();
                    count = page.Key.Total;
                    var construction = page.Select(p => p);
                    return construction.ToList();
                }
            }
            else
            {
                var page = _dbContext.DictionaryConstructionType.OrderBy(p => p.ConstructionTypeId)
                               .Select(p => new DictionaryConstructionTypeModel()
                               {
                                   ConstructionTypeId = p.ConstructionTypeId,
                                   Name = p.Name,
                                   Code = p.Code,
                                   Description = p.Description
                               })
                               .Skip(rowsToSkip).Take(pageSize)
                               .GroupBy(p => new { Total = _dbContext.DictionaryConstructionType.Count() })
                               .First();
                count = page.Key.Total;
                var construction = page.Select(p => p);
                return construction.ToList();
            }
            count = 0;
            return new List<DictionaryConstructionTypeModel>();
        }
        public List<DictionaryConstructionTypeModel> GetDictionaryConstructionTypeModels(int currentPage, int pageSize, out int count)
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

        public void DestroyCounty(CountyModel countyToDestroy)
        {
            var db = _dbContext;



            var county = db.County.Where(pd => pd.CountyId == countyToDestroy.CountyId);



            foreach (var c in county)
            {
                db.County.Remove(c);
            }



            db.SaveChanges();
        }
        public County editCounty(int id)
        {
            return _dbContext.County.First(a => a.CountyId == id);
        }

        public CountyModel GetCountyInfoById(int id)
        {
            CountyModel county = new CountyModel();
            List<CountyModel> counties = new List<CountyModel>();
            var query = _dbContext.County.Where(a => a.CountyId.Equals(id)).Include(b => b.Country);
            if (query.Count() == 1)
            {
                var page = query.Select(a => new CountyModel
                {
                    CountyId = a.CountyId,
                    Name = a.Name,
                    Code = a.Code,
                    CountryId = a.Country.CountryId,
                    CountryName = a.Country.Name
                });
                counties = page.ToList();
            }
            if (counties.Count == 1)
            {
                county = counties[0];
            }
            return county;
        }

        public void DestroyConstruction(DictionaryConstructionTypeModel constructionToDestroy)
        {
            var db = _dbContext;



            var construction = db.DictionaryConstructionType.Where(pd => pd.ConstructionTypeId == constructionToDestroy.ConstructionTypeId);



            foreach (var c in construction)
            {
                db.DictionaryConstructionType.Remove(c);
            }



            db.SaveChanges();
        }
        public int UpdateConstruction(DictionaryConstructionType ConstructionToUpdate)
        {
            var db = _dbContext;
            if (ConstructionToUpdate.ConstructionTypeId == 0)
            {
                db.DictionaryConstructionType.Add(ConstructionToUpdate);
                db.SaveChanges();
            }
            else
            {
                //update
            }
            return 0;
        }
        public DictionaryConstructionType GetConstructionById(int Id)
        {



            var query = from ct in _dbContext.DictionaryConstructionType
                        where ct.ConstructionTypeId == Id
                        select ct;



            DictionaryConstructionType selectedConstruction = query.FirstOrDefault();
            return selectedConstruction;



        }
    }
}
