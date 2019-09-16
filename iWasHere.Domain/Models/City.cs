using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class City
    {
        public City()
        {
            Landmark = new HashSet<Landmark>();
        }

        public int CityId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CountyId { get; set; }

        public virtual County County { get; set; }
        public virtual ICollection<Landmark> Landmark { get; set; }
    }
}
