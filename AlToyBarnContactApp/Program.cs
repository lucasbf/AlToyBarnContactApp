using AlToyBarnContactApp.Models;
using AlToyBarnContactApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Carrega a string de conexão e a injeta no ContatosContext
builder.Services.AddDbContext<ContatosContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("ContatosDatabase")
    )
);

// Injeção de dependências
// builder.Services.AddScoped<IClienteService, DBContextCliente>();
builder.Services.AddScoped<IClienteService, DapperCliente>();
//builder.Services.AddScoped<IContatoService, DBContextContato>();
builder.Services.AddScoped<IContatoService, DapperContato>();

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
