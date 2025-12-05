using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UtilityManagementSystem.Models;
using UtilityManagementSystem.Services;
using UtilityManagementSystem.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UtilityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<UtilityDbContext>()
    .AddDefaultTokenProviders();

// Generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Domain services
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IMetersService, MetersService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IPaymentsService, PaymentsService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IComplaintsService, ComplaintsService>();
builder.Services.AddScoped<IMeterReadingsService, MeterReadingsService>();
builder.Services.AddScoped<ITariffsService, TariffsService>();


builder.Services.AddScoped<IDashboardService, DashboardService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
    var db = services.GetRequiredService<UtilityDbContext>();

    await IdentitySeed.SeedRoles(roleManager);     // first, make sure roles exist
    await IdentitySeed.SeedEmployees(userManager, db); // then create users from Employee table
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
