using System.Reflection;
using Common.Options;
using MassTransit;
using MessageQueue;
using MessageQueue.Constants;
using MessageQueue.Messages.Profile;
using MessageQueue.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Profile.BusinessLogic.Consumers;
using Profile.BusinessLogic.InfrastructureServices.Implementations;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;
using Profile.BusinessLogic.Services.Implementations;
using Profile.BusinessLogic.Services.Interfaces;

namespace Profile.BusinessLogic.DI;

public static class DependencyInjection
{
    public static void RegisterBusinessLogic(this IServiceCollection services, IConfiguration config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.RegisterServices();
        services.ConfigureApplicationOptions(config);
        services.RegisterMassTransit();
        services.ConfigureMinioOptions(config);
        services.ConfigureMinio(config);
    }

    private static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILanguageService, LanguageService>();
        services.AddScoped<IInterestService, InterestService>();
        services.AddScoped<IEducationService, EducationService>();
        services.AddScoped<IImageService, ImageService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IDbCleanupService, DbCleanUpService>();
        services.AddSingleton<ICacheService, CacheService>();
    }

    private static void ConfigureMinioOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<MinioOptions>(config.GetSection("Minio"));
    }
    
    private static void ConfigureMinio(this IServiceCollection services, IConfiguration config)
    {
        var minioOptions = services.BuildServiceProvider().GetService<IOptions<MinioOptions>>()!.Value;
        
        services.AddSingleton<IMinioService>(provider => new MinioService(minioOptions.Endpoint, minioOptions.AccessKey, 
            minioOptions.SecretKey, minioOptions.BucketName));
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
                    if (messageConfig.MessageType is MessageTypeConstants.ProfileMessages.ProfileCreated)
                    {
                        rabbitConfig.Message<ProfileCreatedEventMessage>(m => m.SetEntityName(messageConfig.Exchange));
                        rabbitConfig.Publish<ProfileCreatedEventMessage>(e =>
                        {
                            e.ExchangeType = messageConfig.ExchangeType;
                            e.Durable = true;
                        });
                        rabbitConfig.Send<ProfileCreatedEventMessage>(e =>
                        {
                            e.UseRoutingKeyFormatter(c => c.Message.RoutingKey);
                        });
                    }
                    
                    if (messageConfig.MessageType is MessageTypeConstants.ProfileMessages.ProfileUpdated)
                    {
                        rabbitConfig.Message<ProfileUpdatedEventMessage>(m => m.SetEntityName(messageConfig.Exchange));
                        rabbitConfig.Publish<ProfileUpdatedEventMessage>(e =>
                        {
                            e.ExchangeType = messageConfig.ExchangeType;
                            e.Durable = true;
                        });
                        rabbitConfig.Send<ProfileUpdatedEventMessage>(e =>
                        {
                            e.UseRoutingKeyFormatter(c => c.Message.RoutingKey);
                        });
                    }
                    
                    if (messageConfig.MessageType is MessageTypeConstants.ProfileMessages.ProfileDeleted)
                    {
                        rabbitConfig.Message<ProfileDeletedEventMessage>(m => m.SetEntityName(messageConfig.Exchange));
                        rabbitConfig.Publish<ProfileDeletedEventMessage>(e =>
                        {
                            e.ExchangeType = messageConfig.ExchangeType;
                            e.Durable = true;
                        });
                        rabbitConfig.Send<ProfileDeletedEventMessage>(e =>
                        {
                            e.UseRoutingKeyFormatter(c => c.Message.RoutingKey);
                        });
                    }
                    
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
                                case MessageTypeConstants.UserMessages.UserCreated:
                                {
                                    consumerType = typeof(UserCreatedEventConsumer);
                                    break;
                                }
                                case MessageTypeConstants.UserMessages.UserDeleted:
                                {
                                    consumerType = typeof(UserDeletedEventConsumer);
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
        
        
        services.TryAddScoped<ICommunicationBus, CommunicationBus>();
    }
    
    private static void ConfigureApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptionsWithValidateOnStart<RabbitMqOptions>()
            .ValidateDataAnnotations()
            .Bind(configuration.GetSection(RabbitMqOptions.ConfigurationSectionName));
    }
}