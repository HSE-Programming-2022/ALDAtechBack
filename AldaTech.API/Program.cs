using Microsoft.EntityFrameworkCore;
using AldaTech_api.Models;
using System.Net;

using AldaTech_api.BotCore;


var builder = WebApplication.CreateBuilder(args);

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("127.0.0.1:8080",
                "http://localhost:8080");
            policy.AllowAnyHeader();
        });
});
// Подключаем БД
string connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<BotRunner>();

// builder.Services.AddSingleton<TelegramAPI>();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHealthChecks();
// builder.Services.AddHttpsRedirection(options =>
// {
//     options.RedirectStatusCode = (int)HttpStatusCode.TemporaryRedirect;
//     options.HttpsPort = 5001;
// });

var app = builder.Build();
app.MapHealthChecks("/health");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/", (ApplicationContext db) => db.Users.ToList());

app.Run();
