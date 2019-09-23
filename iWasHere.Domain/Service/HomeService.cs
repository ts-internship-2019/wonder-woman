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
    }
}
