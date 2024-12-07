using Microsoft.EntityFrameworkCore;
using Portfolio.DatabaseConnection.Data;
using Portfolio.RepositoryConfig.IRepositories;
using Portfolio.RepositoryConfig.Repositories;
using Portfolio.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// builder.Services.AddDbContext<PortfolioDbContext>(options =>
//     options.UseNpgsql(builder.Configuration.GetConnectionString("PostGresLocalDatabase")));
// Add services SQL to the container.
builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDatabase")));
//Add IUnitOfWork service
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5211); // HTTP
    options.ListenLocalhost(7013, listenOptions => listenOptions.UseHttps()); // HTTPS
});

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
            "https://localhost:7013",
            "https://localhost:5211",
            "http://localhost:3000",
            "http://localhost:3001",
            "http://tanvirul-001-site1.mtempurl.com/"
            ) // Add allowed origins
              .AllowAnyHeader()  // Allow specific headers or use .WithHeaders()
              .AllowAnyMethod(); // Allow specific methods or use .WithMethods()
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
    });
}

// Redirect root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
