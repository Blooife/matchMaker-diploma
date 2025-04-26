using System.Reflection;
using MassTransit;
using Match.BusinessLogic.Consumers;
using Match.BusinessLogic.Hubs;
using Match.BusinessLogic.Services.Implementations;
using Match.BusinessLogic.Services.Interfaces;
using MessageQueue.Constants;
using MessageQueue.Messages.Profile;
using MessageQueue.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Match.BusinessLogic.DI;

public static class DependencyInjection
{
    public static void RegisterBusinessLogic(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.RegisterServices();
        services.ConfigureApplicationOptions(configuration);
        services.RegisterMassTransit();
    }
    
    private static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IMatchService, MatchService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddSingleton<IConnectionManager, ConnectionManager>();

    }
    
    private static void RegisterMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, rabbitConfig) =>
            {
                var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                rabbitConfig.Host(rabbitMqOptions.HostName, rabbitMqOptions.VirtualHost, hostOptions =>
                {
                    hostOptions.Username(rabbitMqOptions.UserName);
                    hostOptions.Password(rabbitMqOptions.Password);
                });

                if (rabbitMqOptions.Messages is null)
                {
                    return;
                }
                
                foreach (var messageConfig in rabbitMqOptions.Messages)
                {
                    if (messageConfig.QueueName is not null)
                    {
                        rabbitConfig.ReceiveEndpoint(messageConfig.QueueName, re =>
                        {
                            re.ConfigureConsumeTopology = false;

                            re.Bind(messageConfig.Exchange, e =>
                            {
                                e.RoutingKey = messageConfig.RoutingKey;
                                e.ExchangeType = messageConfig.ExchangeType;
                            });
                            Type? consumerType = null;
                            switch (messageConfig.MessageType)
                            {
                                case MessageTypeConstants.ProfileMessages.ProfileCreated:
                                {
                                    consumerType = typeof(ProfileCreatedEventConsumer);
                                    break;
                                }
                                case MessageTypeConstants.ProfileMessages.ProfileDeleted:
                                {
                                    consumerType = typeof(ProfileDeletedEventConsumer);
                                    break;
                                }
                                case MessageTypeConstants.ProfileMessages.ProfileUpdated:
                                {
                                    consumerType = typeof(ProfileUpdatedEventConsumer);
                                    break;
                                }
                            }

                            if (consumerType != null)
                            {
                                re.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                                re.ConfigureConsumer(context, consumerType);
                            }
                        });
                    }
                }
            });

            x.AddConsumers(Assembly.GetExecutingAssembly());
        });
    }
    
    private static void ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsWithValidateOnStart<RabbitMqOptions>()
            .ValidateDataAnnotations()
            .Bind(configuration.GetSection(RabbitMqOptions.ConfigurationSectionName));
    }
}