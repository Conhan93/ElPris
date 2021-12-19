namespace CloudWebService.Models
{
    public class SelectDeviceData
    {
        public Device Device { get; set; }

        public List<data> datas { get; set; }


        public struct data
        {
            public DateTime time_stamp { get; set; }
            public double price { get; set; }
            public string price_area { get; set; }
            public string unit { get; set; }
            public double measurement { get; set; }
        }
    }
}
