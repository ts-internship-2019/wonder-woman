using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class Country
    {
        public Country()
        {
            County = new HashSet<County>();
            DictionaryCurrencyType = new HashSet<DictionaryCurrencyType>();
            Landmark = new HashSet<Landmark>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<County> County { get; set; }
        public virtual ICollection<DictionaryCurrencyType> DictionaryCurrencyType { get; set; }
        public virtual ICollection<Landmark> Landmark { get; set; }
    }
}
