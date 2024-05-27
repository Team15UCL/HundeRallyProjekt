using HundeRallyWebApp.Models;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Security.Claims;

namespace HundeRallyWebApp.Controllers
{
	[Authorize]
	public class TrackController : Controller
	{
		private readonly IMongoCollection<Exercise> db;
		private static readonly string[] secrets = ["team15"];

		public TrackController()
		{
			var client = new MongoClient("mongodb+srv://askelysgaard:1234@cluster0.avn6de9.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
			db = client.GetDatabase("RallyBane").GetCollection<Exercise>("Exercise");
		}


		public IActionResult Index()
		{
			ViewBag.UserName = User.FindFirst(ClaimTypes.Name)!.Value;
			ViewBag.UserRole = User.FindFirst(ClaimTypes.Role)!.Value;

			var token = JwtBuilder.Create()
								  .WithAlgorithm(new HMACSHA256Algorithm())
								  .WithSecret(secrets)
								  .AddClaim("UserName", User.FindFirst(ClaimTypes.Name)!.Value)
								  .AddClaim("UserRole", User.FindFirst(ClaimTypes.Role)!.Value)
								  .Encode();

			ViewBag.Token = token;

			var filter = Builders<Exercise>.Filter.Empty;
			var nodes = db.Find(filter).ToList().OrderBy(x => x.Number).ToList();

			return View(nodes);
		}
	}
}
