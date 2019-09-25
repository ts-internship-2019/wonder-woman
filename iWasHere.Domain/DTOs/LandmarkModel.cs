using System;
using System.Collections.Generic;
using System.Text;

namespace iWasHere.Domain.DTOs
{
    public class LandmarkModel
    {
        public int LandmarkId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Descr { get; set; }
        public int? ConstructionTypeId { get; set; }
        public int? HistoricalPeriodTypeId { get; set; }
        public int? LandmarkTypeId { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? CountryId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
        public string MapUrl { get; set; }
        public string CommentText { get; set; }
        public string CommentTitle { get; set; }
        public string CommentOwner { get; set; }
        public int? CommentId { get; set; }
    }
}
