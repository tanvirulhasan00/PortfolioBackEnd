using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Portfolio.DatabaseConfig.Data;
using Portfolio.RepositoryConfig.IRepositories;
using Portfolio.RepositoryConfig.Repositories;
using Portfolio.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
        "Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\n" +
        "Example: \"Bearer 1234asdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "Portfolio v1",
        Description = "Api to manage portfolio",
        TermsOfService = new Uri("https://logicninja.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Logic Ninja",
            Url = new Uri("https://logicninja.com")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://logicninja.com/license")
        }
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Portfolio v2",
        Description = "Api to manage portfolio",
        TermsOfService = new Uri("https://logicninja.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Logic Ninja",
            Url = new Uri("https://logicninja.com")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://logicninja.com/license")
        }
    });
});

builder.Services.AddDbContext<PortfolioDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDatabase")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var key = builder.Configuration.GetValue<string>("TokenSetting:SecretKey") ?? "";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false,

    };
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
            "http://tanvirul-001-site1.mtempurl.com/",
            "http://localhost:5173/"
            ) // Add allowed origins
              .AllowAnyHeader()  // Allow specific headers or use .WithHeaders()
              .AllowAnyMethod() // Allow specific methods or use .WithMethods()
              .AllowAnyOrigin();
    });

    options.AddPolicy("AllowSpecificOrigins", policy =>
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
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Portfolio API V2");
    });
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Portfolio API V2");
    });
}

// Redirect root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

