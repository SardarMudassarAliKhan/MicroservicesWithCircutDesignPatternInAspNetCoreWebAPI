using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_BAL.IService;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_BAL.Service;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.AppDbContext;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.IRepository;
using MicroservicesWithCircutDesignPatternInAspNetCoreWebAPI_DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); // Replace with your database provider and connection string
});
builder.Services.AddTransient<IBookService, BookService>();
builder.Services.AddTransient<IBookRepository, BookRepository>();
builder.Services.AddHttpClient<IBookService, BookService>(client =>
{
    client.BaseAddress = new Uri("Your_Base_Address_Url"); // Set your base URL here
})
.AddPolicyHandler(GetCircuitBreakerPolicy()); // Apply Circuit Breaker policy

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices Development using Circut Breaker Design Pattern", Version = "v1" });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
// Define Circuit Breaker policy
IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
}