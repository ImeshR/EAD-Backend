/*
 * File: Program.cs
 * Author: Fernando B.K.M.
 * Description:  sets up the web host, defines configurations, and links to the Startup class
  where the bulk of application configurations and services are configured.

*/
using System.Security.Claims;
using System.Text;
using EAD_Backend.Data;
using EAD_Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization : `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type=ReferenceType.SecurityScheme,
                    Id= JwtBearerDefaults.AuthenticationScheme
                }
            }, new string []{}
        }
    });
});

// Dipensancy Injection
builder.Services.AddSingleton<MongoDBService>(); // Singleton creates one instance for the lifetime of the application
builder.Services.AddScoped<UserService>(); // Scoped creates one instance per request
builder.Services.AddScoped<ProductService>(); // Scoped creates one instance per request
builder.Services.AddScoped<MasterDataService>(); // Scoped creates one instance per request
builder.Services.AddScoped<OrderServices>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<OrderTrackingService>();
builder.Services.AddScoped<PaymentService>();


// JWT Configuration
builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});
// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Apply CORS policy
app.UseCors("AllowAllOrigins");

// Middleware
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

// No need for app.Run() with URL, it will pick it from launchSettings.json
app.Run();
