using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class DictionarySeasonType
    {
        public DictionarySeasonType()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int SeasonTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
