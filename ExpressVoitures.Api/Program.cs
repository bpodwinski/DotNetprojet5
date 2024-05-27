using System.Reflection;
using ExpressVoituresApi.Repositories.Interfaces;
using ExpressVoituresApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    // Configure Swagger to use the XML comments file
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Swagger/OpenAPI https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with SQL Server
builder.Services.AddDbContext<ExpressVoituresApi.Data.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories and services
builder.Services.AddScoped<IVehicleRepository, ExpressVoituresApi.Repositories.VehicleRepository>();
builder.Services.AddScoped<IPurchaseRepository, ExpressVoituresApi.Repositories.PurchaseRepository>();
builder.Services.AddScoped<IRepairRepository, ExpressVoituresApi.Repositories.RepairRepository>();
builder.Services.AddScoped<ISaleRepository, ExpressVoituresApi.Repositories.SaleRepository>();
builder.Services.AddScoped<IVehicleService, ExpressVoituresApi.Services.VehicleService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();