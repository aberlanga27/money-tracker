namespace MoneyTracker.API.Config;

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyTracker.API.Filters;
using MoneyTracker.Domain.Entities.Config;

/// <summary>
/// Swagger configuration
/// </summary>
[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    /// <summary>
    /// Add the Swagger configuration
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, MoneyTrackerSettings appSettings)
    {
        services.AddSwaggerGen(c =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            c.OperationFilter<HttpHeadersOperationFilter>();
        });

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Jwt.Issuer,
                    ValidAudience = appSettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt?.Key ?? string.Empty))
                };
            });

        return services;
    }

    /// <summary>
    /// Use the Swagger UI
    /// </summary>
    /// <param name="app"></param>
    /// <param name="appSettings"></param>
    /// <returns></returns>
    public static WebApplication AddSwaggerUI(this WebApplication app, MoneyTrackerSettings appSettings)
    {
        app.UseSwagger(c =>
        {
            c.PreSerializeFilters.Add((swagger, httpReq) =>
            {
                swagger.Info = new OpenApiInfo
                {
                    Title = "MoneyTracker API",
                    Version = $"{appSettings.Environment?.Name?.ToUpper(CultureInfo.InvariantCulture)} v1",
                    Description = "MoneyTracker API",
                    Contact = new OpenApiContact
                    {
                        Name = "Dev Team",
                        Email = "TBD"
                    }
                };
            });
        });

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MoneyTracker.API v1");
            c.RoutePrefix = string.Empty;
        });

        return app;
    }
}