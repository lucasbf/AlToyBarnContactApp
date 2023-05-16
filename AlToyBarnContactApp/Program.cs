using AlToyBarnContactApp.Models;
using AlToyBarnContactApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Carrega a string de conex�o e a injeta no ContatosContext
builder.Services.AddDbContext<ContatosContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ContatosDatabase")
    )
);

builder.Services.AddScoped<IClienteService, DBContextCliente>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();