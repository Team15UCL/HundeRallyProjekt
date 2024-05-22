using HundeRallyIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace HundeRallyIdentity.Controllers
{
	[Authorize]
	public class TrackController : Controller
	{
		private readonly IMongoCollection<Exercise> db;

		public TrackController()
		{
			var client = new MongoClient("mongodb+srv://askelysgaard:1234@cluster0.avn6de9.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
			db = client.GetDatabase("RallyBane").GetCollection<Exercise>("Exercise");
		}


		public IActionResult Index()
		{
			ViewBag.UserName = User.FindFirst(ClaimTypes.Name)!.Value;
			ViewBag.UserRole = User.FindFirst(ClaimTypes.Role)!.Value;

			var filter = Builders<Exercise>.Filter.Empty;
			var nodes = db.Find(filter).ToList().OrderBy(x => x.Number).ToList();

			return View(nodes);
		}
	}
}
