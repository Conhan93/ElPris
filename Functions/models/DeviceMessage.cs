using System;
using System.Collections.Generic;
using System.Text;

namespace IoTCloud_AF.models
{
    class DeviceMessage
    {
        public string adress { get; set; }
        public string alias { get; set; }
        public DateTime timestamp { get; set; }
        public float measurement { get; set; }
    }
}
