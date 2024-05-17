using HundeRallyIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Security.Claims;

namespace HundeRallyIdentity.Controllers
{
    public class TrackController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        
        public TrackController(UserManager<IdentityUser> usermanager)
        {
            _userManager = usermanager;
        }


        public async Task<IActionResult> Index()
        {
            var client = new MongoClient("mongodb+srv://askelysgaard:1234@cluster0.avn6de9.mongodb.net/?retryWrites=true&w=majority&appName=Cluster0");
            var db = client.GetDatabase("RallyBane").GetCollection<Exercise>("Exercise");
            var users = await _userManager.Users.ToListAsync();
            ViewBag.UserName = User.FindFirst(ClaimTypes.Name).Value;
            ViewBag.UserRole = User.FindFirst(ClaimTypes.Role).Value;
            var filter = Builders<Exercise>.Filter.Empty;
            var nodes = db.Find(filter).ToList().OrderBy(x => x.Number).ToList();
            
            return View(nodes);
        }
    }
}
