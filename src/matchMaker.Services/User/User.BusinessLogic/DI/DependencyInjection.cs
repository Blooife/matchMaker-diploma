using System.Reflection;
using Common.Authorization.Context;
using MassTransit;
using MessageQueue;
using MessageQueue.Constants;
using MessageQueue.Messages.User;
using MessageQueue.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using User.BusinessLogic.Providers.Implementations;
using User.BusinessLogic.Providers.Interfaces;
using User.BusinessLogic.Services.Implementations;
using User.BusinessLogic.Services.Interfaces;

namespace User.BusinessLogic.DI;

public static class DependencyInjection
{
    public static void ConfigureBusinessLogic(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureServices();
        services.ConfigureProviders();
        services.ConfigureApplicationOptions(config);
        services.RegisterProducers(config);
    }
    
    private static void ConfigureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserReportService, UserReportService>();
        
        services.TryAddScoped<IAuthenticationContext, AuthenticationContext>();
    }
    
    private static void ConfigureProviders(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();
        services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();
    }

    private static void ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsWithValidateOnStart<RabbitMqOptions>()
            .ValidateDataAnnotations()
            .Bind(configuration.GetSection(RabbitMqOptions.ConfigurationSectionName));
    }
    
    private static void RegisterProducers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, rabbitConfig) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                rabbitConfig.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHost, hostOptions =>
                {
                    hostOptions.Username(rabbitMqOptions.UserName);
                    hostOptions.Password(rabbitMqOptions.Password);
                });

                rabbitConfig.UseMessageRetry(retryConfig =>
                {
                    retryConfig.Interval(3, TimeSpan.FromSeconds(10));
                });

                foreach (var messageConfig in rabbitMqOptions.Messages)
                {
                    if (messageConfig.MessageType is MessageTypeConstants.UserMessages.UserCreated)
                    {
                        rabbitConfig.Message<UserCreatedEventMessage>(m => m.SetEntityName(messageConfig.Exchange));
                        rabbitConfig.Publish<UserCreatedEventMessage>(e =>
                        {
                            e.ExchangeType = messageConfig.ExchangeType;
                            e.Durable = true;
                        });
                        rabbitConfig.Send<UserCreatedEventMessage>(e =>
                        {
                            e.UseRoutingKeyFormatter(c => c.Message.RoutingKey ?? messageConfig.RoutingKey);
                        });
                    }
                    if (messageConfig.MessageType is MessageTypeConstants.UserMessages.UserDeleted)
                    {
                        rabbitConfig.Message<UserDeletedEventMessage>(m => m.SetEntityName(messageConfig.Exchange));
                        rabbitConfig.Publish<UserDeletedEventMessage>(e =>
                        {
                            e.ExchangeType = messageConfig.ExchangeType;
                            e.Durable = true;
                        });
                        rabbitConfig.Send<UserDeletedEventMessage>(e =>
                        {
                            e.UseRoutingKeyFormatter(c => c.Message.RoutingKey ?? messageConfig.RoutingKey);
                        });
                    }
                    
                    if (messageConfig.MessageType is MessageTypeConstants.UserMessages.NotificationCreated)
                    {
                        rabbitConfig.Message<NotificationCreatedEventMessage>(m => m.SetEntityName(messageConfig.Exchange));
                        rabbitConfig.Publish<NotificationCreatedEventMessage>(e =>
                        {
                            e.ExchangeType = messageConfig.ExchangeType;
                            e.Durable = true;
                        });
                        rabbitConfig.Send<NotificationCreatedEventMessage>(e =>
                        {
                            e.UseRoutingKeyFormatter(c => c.Message.RoutingKey ?? messageConfig.RoutingKey);
                        });
                    }
                }
            });
        });

        services.TryAddScoped<ICommunicationBus, CommunicationBus>();
    }
}