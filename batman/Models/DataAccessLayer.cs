using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace batman.Models
{
    public class DataAccessLayer
    {
        DatabaseContext dbContext = new DatabaseContext();

        public IEnumerable<DictionaryLandmarkType> GetAllLandmarks()
        {
            try
            {
                return dbContext.DictionaryLandmarkType.ToList();
            }
            catch
            {
                throw;
            }
        }

        public bool AddDictionaryLandmarkType(DictionaryLandmarkType dictionaryLandmark)
        {
            try
            {
                dbContext.DictionaryLandmarkType.Add(dictionaryLandmark);
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}

