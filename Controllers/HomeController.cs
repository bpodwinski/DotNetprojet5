using System.Diagnostics;
using ExpressVoitures.Data;
using ExpressVoitures.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressVoitures.Controllers
{
	[IgnoreAntiforgeryToken]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		[IgnoreAntiforgeryToken]
		public IActionResult Index()
		{
			return View();
		}

		[IgnoreAntiforgeryToken]
		public async Task<IActionResult> Adverts()
		{
			var applicationDbContext = _context.Vehicle
				.Include(v => v.Brand)
				.Include(v => v.Model)
				.Include(v => v.TrimLevel)
				.Include(v => v.Repairs);

			return View(await applicationDbContext.ToListAsync());
		}

		[IgnoreAntiforgeryToken]
		[HttpGet("/privacy")]
		public IActionResult Privacy()
		{
			return View();
		}

		[IgnoreAntiforgeryToken]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
    }
}
