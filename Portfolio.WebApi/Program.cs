using Microsoft.EntityFrameworkCore;
using Portfolio.DatabaseConfig.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PortfolioDbContext>
    (options=>options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDatabase")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=>{
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
    });
}
// Redirect root URL to Swagger UI
app.MapGet("/", context => 
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.UseHttpsRedirection();

app.MapControllers();

app.Run();

