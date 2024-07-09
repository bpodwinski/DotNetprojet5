using System.Diagnostics;
using ExpressVoituresV2.Data;
using ExpressVoituresV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoituresV2.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Adverts()
		{
			var applicationDbContext = _context.Vehicle
				.Include(v => v.Brand)
				.Include(v => v.Model)
				.Include(v => v.TrimLevel)
				.Include(v => v.Repairs);

			return View(await applicationDbContext.ToListAsync());
		}

		[HttpGet("/privacy")]
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
