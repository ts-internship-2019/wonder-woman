using System;
using System.Collections.Generic;
using System.Text;

namespace iWasHere.Domain.DTOs
{
    public class CityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CountyId { get; set; }
        public string CountyName { get; set; }
    }
}
