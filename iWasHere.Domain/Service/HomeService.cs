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
        public void UpdateLandmark(LandmarkModel lm, out string errorMessage, out int id)
        {

            Landmark landmark = new Landmark();
            if (lm.LandmarkId != 0)
                landmark.LandmarkId = lm.LandmarkId;
            if (!(string.IsNullOrWhiteSpace(lm.Code)))
                landmark.Code = lm.Code;
            if (!(string.IsNullOrWhiteSpace(lm.Name)))
                landmark.Name = lm.Name;
            if (!string.IsNullOrWhiteSpace(lm.Descr))
                landmark.Descr = lm.Descr;
            if (lm.ConstructionTypeId != null)
                landmark.ConstructionTypeId = lm.ConstructionTypeId;
            if (lm.LandmarkTypeId != null)
                landmark.LandmarkTypeId = lm.LandmarkTypeId;
            if (lm.Latitude != null)
                landmark.Latitude = lm.Latitude;
            if (lm.Longitude != null)
                landmark.Longitude = lm.Longitude;
            if (lm.CountryId != null)
                landmark.CountryId = lm.CountryId;
            if (lm.CountyId != null)
                landmark.CountyId = lm.CountyId;
            if (lm.CityId != null)
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
            id = landmark.LandmarkId;
        }

        public List<DictionaryLandmarkType> GetLandmarks(string text)
        {
            var query = _dbContext.DictionaryLandmarkType.Select(c => new DictionaryLandmarkType()
            {
                LandmarkTypeId = c.LandmarkTypeId,
                Name = c.Name,
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }
        public void SaveImagesDB(string path, int id)
        {
           
            Photo photo = new Photo()
            {
                ImagePath = "~/images/" + path,
                LandmarkId = id
            };

            _dbContext.Photo.Add(photo);

            _dbContext.SaveChanges();

        }
        public string DeleteImagesDB( int id)
        {

            Photo deleted = _dbContext.Photo.First(a => a.PhotoId == id);

            _dbContext.Photo.Remove(deleted);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(ex.ToString()))
                    return ex.ToString();
            }
            return null;

        }

        public List<CityModel> GetCities(string text)
        {
            var query = _dbContext.City.Select(c => new CityModel()
            {
                Id = c.CityId,
                Name = c.Name
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }

        public List<DictionaryConstructionTypeModel> GetConstructions(string text)
        {
            var query = _dbContext.DictionaryConstructionType.Select(c => new DictionaryConstructionTypeModel()
            {
                ConstructionTypeId = c.ConstructionTypeId,
                Name = c.Name
            }).Where(c => c.Name.Contains(text)).Take(100);
            return query.ToList();
        }
    }
}
