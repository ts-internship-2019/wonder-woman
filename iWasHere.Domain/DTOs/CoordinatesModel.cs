using System;
using System.Collections.Generic;
using System.Text;

namespace iWasHere.Domain.DTOs
{
    public class CoordinatesModel
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        string Url { get; set; }
    }
}
