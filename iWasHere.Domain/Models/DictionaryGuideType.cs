using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class DictionaryGuideType
    {
        public DictionaryGuideType()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int GuideTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
