using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class DictionaryCurrencyType
    {
        public DictionaryCurrencyType()
        {
            ExchangeRates = new HashSet<ExchangeRates>();
            Ticket = new HashSet<Ticket>();
        }

        public int CurrencyTypeId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CurrencyCountryId { get; set; }
        public string Description { get; set; }

        public virtual Country CurrencyCountry { get; set; }
        public virtual ICollection<ExchangeRates> ExchangeRates { get; set; }
        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
