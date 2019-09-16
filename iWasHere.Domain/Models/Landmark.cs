using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class Landmark
    {
        public Landmark()
        {
            Comment = new HashSet<Comment>();
            LandmarkXticket = new HashSet<LandmarkXticket>();
            Photo = new HashSet<Photo>();
        }

        public int LandmarkId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? ConstructionTypeId { get; set; }
        public int? HistoricalPeriodTypeId { get; set; }
        public int? LandmarkTypeId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? CountryId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }

        public virtual City City { get; set; }
        public virtual DictionaryConstructionType ConstructionType { get; set; }
        public virtual Country Country { get; set; }
        public virtual County County { get; set; }
        public virtual DictionaryHistoricalPeriodType HistoricalPeriodType { get; set; }
        public virtual DictionaryLandmarkType LandmarkType { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<LandmarkXticket> LandmarkXticket { get; set; }
        public virtual ICollection<Photo> Photo { get; set; }
    }
}
