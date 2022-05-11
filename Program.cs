using BudgetApp.Context;
using BudgetApp.Controllers;
using BudgetApp.Middleware;
using BudgetApp.Models.Mongo;
using BudgetApp.Services;
using Going.Plaid;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

// Add services to the container.
builder.Configuration.AddJsonFile(
    path: "appsettings.json",
    optional: true,
    reloadOnChange: false
    )
    .AddJsonFile(
    path: "secrets.json",
    optional: true,
    reloadOnChange: false
    );

var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(connectionstring);
    options.EnableSensitiveDataLogging();
});

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<UsersService>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Configure<PlaidCredentials>(builder.Configuration.GetSection(PlaidOptions.SectionKey));
builder.Services.Configure<PlaidOptions>(builder.Configuration.GetSection(PlaidOptions.SectionKey));
builder.Services.AddSingleton<PlaidClient>();

builder.Services.AddScoped<PlaidController>();
builder.Services.AddScoped<DatabaseController>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
