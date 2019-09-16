using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            LandmarkXticket = new HashSet<LandmarkXticket>();
        }

        public int TicketId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool? IsDefault { get; set; }
        public int? TicketTypeId { get; set; }
        public int? CurrencyTypeId { get; set; }
        public int? SeasonTypeId { get; set; }
        public int? GuideTypeId { get; set; }

        public virtual DictionaryCurrencyType CurrencyType { get; set; }
        public virtual DictionaryGuideType GuideType { get; set; }
        public virtual DictionarySeasonType SeasonType { get; set; }
        public virtual DictionaryTicketType TicketType { get; set; }
        public virtual ICollection<LandmarkXticket> LandmarkXticket { get; set; }
    }
}
