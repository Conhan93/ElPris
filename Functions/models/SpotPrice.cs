using System;
using System.Collections.Generic;
using System.Text;

namespace IoTCloud_AF.models
{
    class SpotPrice
    {
        public DateTime TimeStamp { get; set; }
        public DateTime TimeStampDay { get; set; }
        public string TimeStampHour { get; set; }
        public float Value { get; set; }
        public string Unit { get; set; }
    }
}
