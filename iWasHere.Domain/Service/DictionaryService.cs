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
                Name = a.Name
            }).ToList();

            return dictionaryLandmarkTypeModels;
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
