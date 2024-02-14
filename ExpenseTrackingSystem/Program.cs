using ExpenseTrackingSystem.Entities.Models;
using ExpenseTrackingSystem.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using ExpenseTrackingSystem.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ExpenseTrackingSystem.Repositories;


var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile("appsettings.Development.json");

builder.Services.AddDbContext<CustomContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    new MySqlServerVersion(new Version(8, 0, 23))));

var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddScoped<IExpenseService,ExpenseService>();
builder.Services.AddScoped<IExpenseRepository,ExpenseRepository>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,  // Set to true to validate token lifetime
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: jwtSettings["SecretKey"])),
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
        };

     
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                
                Console.WriteLine(context.Exception);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddMvc();

builder.Services.AddIdentity<CustomUser, CustomUserRole>(options =>
{
    options.Password.RequireDigit = true;
})
.AddEntityFrameworkStores<CustomContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SysAdminPolicy", policy => policy.RequireRole("SysAdmin"));
    options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
    options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("Employee"));
});

builder.Services.AddControllers();

builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
            },
           new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Tracking System API V1");

        c.OAuthClientId("swagger-ui");
        c.OAuthClientSecret("swagger-ui-secret");





        c.OAuthRealm("swagger-ui-realm");
        c.OAuthAppName("Swagger UI");
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Map("/api-docs", builder =>
{
    builder.Run(context =>
    {
        var user = context.User;

        // Log user claims and roles
        foreach (var claim in user.Claims)
        {
            Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
        }

        foreach (var role in user.FindAll(ClaimTypes.Role))
        {
            Console.WriteLine($"Role: {role.Value}");
        }

        return Task.CompletedTask;
    });
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<CustomContext>();
    var userManager = services.GetRequiredService<UserManager<CustomUser>>();
    var roleManager = services.GetRequiredService<RoleManager<CustomUserRole>>();

 

    try
    {
       
        dbContext.Database.Migrate();
        await RoleInitializer.InitializeAsync(userManager, roleManager);

        app.Run();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing roles.");
    }
}
