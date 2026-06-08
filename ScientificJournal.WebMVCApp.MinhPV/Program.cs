using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ScientificJournal.Entities.MinhPV.Models;
using ScientificJournal.Repositories.MinhPV;
using ScientificJournal.Services.MinhPV;
using ScientificJournal.Services.MinhPV.Interfaces;
using ScientificJournal.WebMVCApp.MinhPV.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<ScientificJournalTrendDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PublishersRepository>();
builder.Services.AddScoped<JournalsMinhPvRepository>();
builder.Services.AddScoped<CategoriesMinhPvRepository>();
builder.Services.AddScoped<UsersHuyDdRepository>();

builder.Services.AddScoped<IPublishersService, PublishersService>();
builder.Services.AddScoped<IJournalsMinhPvService, JournalsMinhPvService>();
builder.Services.AddScoped<ICategoriesMinhPvService, CategoriesMinhPvService>();
builder.Services.AddScoped<IUsersHuyDdService, UsersHuyDdService>();

builder.Services.AddAuthentication()
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.AccessDeniedPath = new PathString("/Account/Forbidden");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<ScientificJournalHub>("/scientificJournalHub");

app.Run();
