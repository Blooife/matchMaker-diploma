using System.ComponentModel.DataAnnotations;

namespace MessageQueue.Options;

public record RabbitMqOptions
{
    /// <summary>
    /// Наименование секции конфигурации, из которой загружаются настройки RabbitMQ
    /// </summary>
    public static string ConfigurationSectionName { get; set; } = "RabbitMQ";

    /// <summary>
    /// Наименование хоста RabbitMQ
    /// </summary>
    [Required]
    required public string HostName { get; init; }

    /// <summary>
    /// Наименование пользователя для подключения к RabbitMQ
    /// </summary>
    [Required]
    required public string UserName { get; init; }

    /// <summary>
    /// Пароль для подключения к RabbitMQ
    /// </summary>
    [Required]
    required public string Password { get; init; }

    /// <summary>
    /// Виртуальный хост
    /// </summary>
    [Required]
    required public string VirtualHost { get; init; }

    /// <summary>
    /// Список настроек сообщений
    /// </summary>
    public List<RabbitMqMessage>? Messages { get; set; }

    /// <summary>
    /// Класс, описывающий настройки сообщения RabbitMQ
    /// </summary>
    public class RabbitMqMessage
    {
        /// <summary>
        /// Тип сообщения
        /// </summary>
        public string MessageType { get; set; } = null!;

        /// <summary>
        /// Наименование обменника
        /// </summary>
        public string Exchange { get; set; } = null!;

        /// <summary>
        /// Тип обменника
        /// </summary>
        public string ExchangeType { get; set; } = null!;

        /// <summary>
        /// Ключ маршрутизации для сообщения
        /// </summary>
        public string RoutingKey { get; set; } = null!;

        /// <summary>
        /// Наименование очереди
        /// </summary>
        public string? QueueName { get; set; } = null!;
    }
}