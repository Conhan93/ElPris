#nullable disable
using CloudWebService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudWebService.Controllers
{
    public class SpotPricesController : Controller
    {
        private readonly cloudprojContext _context;

        public SpotPricesController(cloudprojContext context)
        {
            _context = context;
        }

        // GET: SpotPrices
        public async Task<IActionResult> Index()
        {

            var prices = from p in _context.SpotPrices select p;

            prices = prices.OrderByDescending(p => p.TimeStamp).Take(42).OrderBy(p => p.TimeStamp);

            return View(await prices.ToListAsync());
        }


    }
}
