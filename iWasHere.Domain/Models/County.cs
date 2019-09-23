using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class County
    {
        public County()
        {
           City = new HashSet<City>();
            Landmark = new HashSet<Landmark>();
        }

        public int CountyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<City> City { get; set; }
        public virtual ICollection<Landmark> Landmark { get; set; }
    }
}
