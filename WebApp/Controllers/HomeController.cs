using CloudWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CloudWebService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly cloudprojContext _context;

        public HomeController(ILogger<HomeController> logger, cloudprojContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var chartData = await _context.Devices.Join(_context.Measurements,
                d => d.Id,
                m => m.DeviceId,
                (d, m) => new HomeChartData{
                    Alias = d.Alias,
                    Adress = d.Adress,
                    Price = m.Price.Price
                }).ToListAsync();

            var distinct = chartData.DistinctBy(item => item.Adress).ToList();

            foreach(var item in chartData)
            {
                var distinctItem = distinct.Find(d => { return d.Adress == item.Adress; });
                distinctItem.Price += item.Price;
            }

            return View(distinct);
        }

        public async Task<IActionResult> SelectDevice(string adress)
        {
            if (adress.Length == 0 || adress == null)
            {
                return NotFound();
            }
            var devicequery = from devices in _context.Devices where devices.Adress == adress select new Device
            {
                Id = devices.Id,
                Adress = devices.Adress,
                Alias = devices.Alias,
            };
            var device = devicequery.Single();
            var deviceid = device.Id;

            if (device == null)
            {
                return NotFound();
            }
            var q = from m in _context.Measurements
                    from p in _context.SpotPrices
                    where m.DeviceId == deviceid
                    && p.Id == m.PriceId
                    select new SelectDeviceData.data
                    {
                        time_stamp = DateTimeOffset.FromUnixTimeSeconds(m.TimeStamp).DateTime,
                        price = p.Price,
                        price_area = p.PriceArea,
                        measurement = m.Measurement1,
                        unit = p.Unit
                    };

            
            var data = new SelectDeviceData
            {
                Device = device,
                datas = q.ToList()
            };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}