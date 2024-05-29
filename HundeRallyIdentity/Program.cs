using HundeRallyWebApp.Data;
using HundeRallyWebApp.Models.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("LocalConnection") ?? throw new InvalidOperationException("Connection string 'LocalConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = true;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 0;

	// Lockout settings.
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
	options.Lockout.MaxFailedAccessAttempts = 5;
	options.Lockout.AllowedForNewUsers = true;

	// User settings.
	options.User.AllowedUserNameCharacters =
	"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
	// Cookie settings
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

	options.LoginPath = "/Identity/Account/Login";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.SlidingExpiration = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseMigrationsEndPoint();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope()) //To access the services we have configured above //add roles by seeding
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	var roles = new[] { "Admin", "Instructor", "Judge", "Handler" };

	foreach (var role in roles)
	{

		if (!await roleManager.RoleExistsAsync(role)) //Check if the role already exists
			await roleManager.CreateAsync(new IdentityRole(role)); // if not, create it

	}

}

using (var scope = app.Services.CreateScope())
{
	var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

	string email = "admin@admin.com";
	string password = "Test1234";
	string username = "Admin";

	if (await userManager.FindByEmailAsync(email) == null)
	{
		var user = new IdentityUser();
		user.Email = email;
		user.UserName = username;

		await userManager.CreateAsync(user, password);

		await userManager.AddToRoleAsync(user, "Admin");
	}


	string email2 = "instr@instr.com";
	string password2 = "Test1234";
	string username2 = "Bob";

	if (await userManager.FindByEmailAsync(email2) == null)
	{
		var user2 = new IdentityUser();
		user2.Email = email2;
		user2.UserName = username2;

		await userManager.CreateAsync(user2, password2);

		await userManager.AddToRoleAsync(user2, "Instructor");
		await userManager.AddClaimAsync(user2, new System.Security.Claims.Claim("userName", "Bob"));
	}


	string email3 = "judge@Judge.com";
	string password3 = "Test1234";
	string username3 = "Judy";

	if (await userManager.FindByEmailAsync(email3) == null)
	{
		var user3 = new IdentityUser();
		user3.Email = email3;
		user3.UserName = username3;

		await userManager.CreateAsync(user3, password3);

		await userManager.AddToRoleAsync(user3, "Judge");
	}

	string email4 = "doglover@handler.com";
	string password4 = "Test1234";
	string username4 = "Kim";

	if (await userManager.FindByEmailAsync(email4) == null)
	{
		var user4 = new IdentityUser();
		user4.Email = email4;
		user4.UserName = username4;

		await userManager.CreateAsync(user4, password4);

		await userManager.AddToRoleAsync(user4, "Handler");
	}

}

app.MapHub<TrackHub>("/trackHub");

app.Run();
