using System;
using System.Collections.Generic;

namespace CloudWebService.Models
{
    public partial class Measurement
    {
        public int Id { get; set; }
        public long TimeStamp { get; set; }
        public long? PriceId { get; set; }
        public int? DeviceId { get; set; }
        public double Measurement1 { get; set; }

        public virtual Device? Device { get; set; }
        public virtual SpotPrice? Price { get; set; }
    }
}
