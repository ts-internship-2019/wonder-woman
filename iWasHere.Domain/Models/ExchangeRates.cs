using System;
using System.Collections.Generic;

namespace iWasHere.Domain.Models
{
    public partial class ExchangeRates
    {
        public int ExchangeRateId { get; set; }
        public decimal Value { get; set; }
        public int? CurrencyTypeId { get; set; }

        public virtual DictionaryCurrencyType CurrencyType { get; set; }
    }
}
