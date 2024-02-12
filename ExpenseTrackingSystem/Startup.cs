using ExpenseTrackingSystem.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using ExpenseTrackingSystem.Entities.Models;
using Microsoft.AspNetCore.Identity;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add DbContext first
        services.AddDbContext<CustomContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                            new MySqlServerVersion(new Version(8, 0, 23))));

        // Configure Identity
        var jwtSettings = Configuration.GetSection("Jwt");

        services.AddIdentity<CustomUser, CustomUserRole>(options =>
        {
            // Configure identity options if needed
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            // ... other options
        })
        .AddEntityFrameworkStores<CustomContext>()
        .AddDefaultTokenProviders();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                };
            });

        // Add authorization policies
        services.AddAuthorization(options =>
        {
            options.AddPolicy("SysAdminPolicy", policy => policy.RequireRole("SysAdmin"));
            options.AddPolicy("ManagerPolicy", policy => policy.RequireRole("Manager"));
            options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("Employee"));
        });

        services.AddControllers();
    }


    public void Configure(IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

        app.Map("/api-docs", builder =>
        {
            builder.Run(async context =>
            {
                var user = context.User;

                if (user.IsInRole("SysAdmin"))
                {
                    context.Response.Redirect("/admin");
                }
                else if (user.IsInRole("Manager"))
                {
                    context.Response.Redirect("/manager");
                }
                else if (user.IsInRole("Employee"))
                {
                    context.Response.Redirect("/employee");
                }
                else
                {
                    context.Response.StatusCode = 403;
                    await context.Response.WriteAsync("Forbidden");
                }
            });
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
