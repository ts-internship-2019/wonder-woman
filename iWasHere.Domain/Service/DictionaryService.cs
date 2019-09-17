﻿using iWasHere.Domain.DTOs;
using iWasHere.Domain.Model;
using iWasHere.Domain.Models;
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
        public List<CityModel> GetAllPagedCities(int skipRows, int pageSize, string filterName, int filterCounty, out int totalRows)
        {           
            var query = _dbContext.City.Where(a => a.Name.Contains(filterName)).Include(b => b.County).Where(b => b.CountyId.Equals(filterCounty));
            totalRows = 0;
            if(query.Count() > 0)
            {
                var page = query.OrderBy(p => p.Name)
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
