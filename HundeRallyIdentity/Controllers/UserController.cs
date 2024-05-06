using HundeRallyIdentity.Data;
using HundeRallyIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HundeRallyIdentity.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ApplicationDbContext _context;

        public UserController(UserManager<IdentityUser> usermanager, ApplicationDbContext context)
        {
            _userManager = usermanager;
            _context = context;
        }

        [Authorize(Roles="Admin")]

        public async Task<IActionResult> UserList()
        {
            var users = await _userManager.Users.ToListAsync();

            // Create a list of UserViewModel
            var userViewModels = new List<UserViewModel>();

            // Iterate through each user
            foreach (var user in users)
            {
                // Retrieve the roles for the user
                var roles = await _userManager.GetRolesAsync(user);

                // Create a new UserViewModel
                var userViewModel = new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = string.Join(",", roles)
                };

                // Add the UserViewModel to the list
                userViewModels.Add(userViewModel);
            }

            // Return the view with the list of UserViewModel
            return View(userViewModels);
        }

        //public IActionResult UserList() Virker iKke pga. det ikke er async?
        //{
        //    var users = _userManager.Users.Select(c => new UserViewModel()
        //    {
        //        UserName = c.UserName,
        //        Email = c.Email,
        //        Role = string.Join(",", await _userManager.GetRolesAsync(c))

        //    }).ToList();

        //    return View(users);
        //}
    }
}
