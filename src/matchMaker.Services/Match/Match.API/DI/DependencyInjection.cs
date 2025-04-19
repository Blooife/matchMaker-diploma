using System.Text;
using Common.Filters;
using Common.Options;
using Match.BusinessLogic.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Profile.Client;

namespace Match.API.DI;

public static class DependencyInjection
{
    public static void RegisterApi(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthorization();
        services.AddSignalR();
        services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
        services
            .AddControllers(x =>
            {
                x.Filters.Add<ValidationFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        services.ConfigureJwtOptions(config);
        services.ConfigureAuthentication(services.BuildServiceProvider().GetService<IOptions<JwtOptions>>()!.Value);
        services.ConfigureCors();
        services.ConfigureSwagger();
        services.RegisterClients(config);
    }
    
    private static void ConfigureJwtOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtOptions>(config.GetSection("ApiSettings:JwtOptions"));
    }
    
    private static void ConfigureAuthentication(this IServiceCollection services, JwtOptions jwtOptions)
    {
        var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwtOpt =>
        {
            jwtOpt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            jwtOpt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
            
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        });
    }

    private static void ConfigureSwagger(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference= new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id=JwtBearerDefaults.AuthenticationScheme
                        }
                    }, new string[]{}
                }
            });
        });
    }
    
    private static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("MyCorsPolicy", builder =>
                builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("X-Pagination"));
        });
    }
    
    private static void RegisterClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IProfileClient, ProfileClient>(serviceProvider =>
        {
            var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var gatewayUrl = configuration["Gateway:Url"];

            return new ProfileClient(httpClientFactory, httpContextAccessor, gatewayUrl!);
        });
    }
}