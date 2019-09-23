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

    }
}
