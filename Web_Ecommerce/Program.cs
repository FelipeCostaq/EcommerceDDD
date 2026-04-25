using ApplicationApp.Interfaces;
using ApplicationApp.OpenApp;
using Domain.Interfaces.Generics;
using Domain.Interfaces.InterfaceProduct;
using Domain.Interfaces.InterfaceServices;
using Domain.Services;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generics;
using Infrastructure.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

const string connectionStringName = "FelipeConnection";

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");
builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ContextBase>();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton(typeof(IGenerics<>), typeof(RepositoryGenerics<>));
builder.Services.AddScoped<IProduct, RepositoryProduct>();   
builder.Services.AddScoped<InterfaceProductApp, AppProduct>();
builder.Services.AddScoped<IServiceProduct, ServiceProduct>();

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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ContextBase>();
        
        context.Database.Migrate();
        
        Console.WriteLine("Banco de dados verificado/criado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao inicializar banco: {ex.Message}");
    }
}

app.Run();
