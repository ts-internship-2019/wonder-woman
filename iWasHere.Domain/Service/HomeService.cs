using iWasHere.Domain.DTOs;
using iWasHere.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace iWasHere.Domain.Service
{
    public class HomeService
    {
        private readonly DatabaseContext _dbContext;
        public HomeService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }
        public List<LandmarkListModel> GetLandmarkListModels()
        {
            List<LandmarkListModel> landmarkList = _dbContext.Landmark.Select(a => new LandmarkListModel()
            {
                LandmarkId = a.LandmarkId,
                Name = a.Name
            }).ToList();
            return landmarkList;
        }
        public LandmarkModel GetLandmarkById(int id)
        {
            var landmark = _dbContext.Landmark.First(a => a.LandmarkId == id);
            LandmarkModel selectedlandmark = new LandmarkModel()
            {
                LandmarkId = landmark.LandmarkId,
                Code = landmark.Code,
                Name = landmark.Name,
                Descr = landmark.Descr,
                ConstructionTypeId = landmark.ConstructionTypeId,
                HistoricalPeriodTypeId = landmark.HistoricalPeriodTypeId,
                LandmarkTypeId = landmark.LandmarkTypeId,
                Latitude = landmark.Latitude,
                Longitude = landmark.Longitude,
                CountryId = landmark.CountryId,
                CountyId = landmark.CountyId,
                CityId = landmark.CityId
            };
            return selectedlandmark;
        }

       public List<String> GetImagesForLandmarkId(int id)
        {
            List<Photo> photopaths = _dbContext.Photo.Where(a => a.LandmarkId == id).Select(a => new Photo()
            {
                ImagePath = a.ImagePath,
            }).ToList();
            List<String> filepaths = new List<String>();
            foreach (Photo ph in photopaths)
            {
                filepaths.Add(ph.ImagePath);
            }
            return filepaths;
        }
        public void UpdateLandmark(LandmarkModel lm, out string errorMessage)
        {

            Landmark landmark = new Landmark();
            if (lm.LandmarkId != 0)
                landmark.LandmarkId = lm.LandmarkId;
            if (!(string.IsNullOrWhiteSpace(lm.Code)))
                landmark.Code = lm.Code;
            if (!(string.IsNullOrWhiteSpace(lm.Name)))
                landmark.Name = lm.Name;
            if (lm.ConstructionTypeId != 0)
                landmark.ConstructionTypeId = lm.ConstructionTypeId;
            if (landmark.HistoricalPeriodTypeId != 0)
                landmark.HistoricalPeriodTypeId = lm.HistoricalPeriodTypeId;
            if (landmark.LandmarkTypeId != 0)
                landmark.LandmarkTypeId = lm.LandmarkTypeId;
            if (landmark.Latitude != 0)
                landmark.Latitude = lm.Latitude;
            if (landmark.Longitude != 0)
                landmark.Longitude = lm.Longitude;
            if (landmark.CountryId != 0)
                landmark.CountryId = lm.CountryId;
            if (landmark.CountyId != 0)
                landmark.CountyId = lm.CountyId;
            if (landmark.CityId != 0)
                landmark.CityId = lm.CityId;
            errorMessage = "";
            if (lm.LandmarkId == 0)
            {
                _dbContext.Landmark.Add(landmark);
            }
            else
            {
                _dbContext.Landmark.Update(landmark);
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                errorMessage = "Salvarea/Editarea nu a putut fi efectuata cu succes! Te rog sa mai incearci o data!";
            }
        }

        public List<LandmarkModel> GetLandmarks(string text)
        {
            var query = _dbContext.Landmark.Select(c => new LandmarkModel()
            {
                LandmarkId = c.LandmarkId,
                Name = c.Name,
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }

        public List<DictionaryCountryModel> GetCountries(string text)
        {
            var query = _dbContext.Country.Select(c => new DictionaryCountryModel()
            {
                CountryId = c.CountryId,
                Name = c.Name
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }
    }
}
