using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class DictionaryConstructionType
    {
        public DictionaryConstructionType()
        {
            Landmark = new HashSet<Landmark>();
        }

        public int ConstructionTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Landmark> Landmark { get; set; }
    }
}
