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
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

const string connectionStringName = "FelipeConnection";

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString(connectionStringName) ?? throw new InvalidOperationException($"Connection string {connectionStringName} not found.");

builder.Services.AddDbContext<ContextBase>(options =>
    options.UseSqlServer(connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 10,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null)
    ).ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

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
    var logger = services.GetRequiredService<ILogger<Program>>();
    int attempt = 10;

    while (attempt > 0)
    {
        try
        {
            logger.LogInformation("Tentando conectar ao banco de dados e aplicar migrations...");
            var context = services.GetRequiredService<ContextBase>();
            
            context.Database.Migrate(); 
            
            logger.LogInformation("Banco de dados verificado/criado com sucesso!");
            break; 
        }
        catch (Exception ex)
        {
            attempt--;
            logger.LogWarning($"Banco ainda não está pronto. Faltam {attempt} tentativas. Aguardando 10 segundos... Erro: {ex.Message}");
            System.Threading.Thread.Sleep(10000); 
        }
    }
}

app.Run();