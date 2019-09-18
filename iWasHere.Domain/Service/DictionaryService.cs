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
    }
}
