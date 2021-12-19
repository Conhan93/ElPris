using System;
using System.Collections.Generic;

namespace CloudWebService.Models
{
    public partial class SpotPrice
    {
        public SpotPrice()
        {
            Measurements = new HashSet<Measurement>();
        }

        public long Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime TimeStampDay { get; set; }
        public int TimeStampHour { get; set; }
        public double Price { get; set; }
        public string PriceArea { get; set; } = null!;
        public string Unit { get; set; } = null!;

        public virtual ICollection<Measurement> Measurements { get; set; }
    }
}
