using EmployePortal.Authentication;
using EmployePortal.Mapping;
using EmployePortal.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).Enrich
    .FromLogContext()
    .CreateLogger();


builder.Services.AddSingleton<IApiKeyValidation, ApiKeyValidation>();
builder.Logging.AddSerilog(logger);

builder.Services.AddAutoMapper(typeof(EmployeeMapping));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(
    
    builder.Configuration.GetConnectionString("AppData")
    
    
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
