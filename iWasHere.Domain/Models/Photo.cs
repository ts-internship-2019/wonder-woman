using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class Photo
    {
        public int PhotoId { get; set; }
        public int? LandmarkId { get; set; }
        public string Name { get; set; }
        public bool? IsDefault { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }

        public virtual Landmark Landmark { get; set; }
    }
}
