using Match.DataAccess.Models;
using Match.DataAccess.Providers.Interfaces;
using MongoDB.Driver;

namespace Match.DataAccess.Providers.Implementations;

public class NotificationRepository(IMongoCollection<Notification> _collection) : GenericRepository<Notification, string>(_collection), INotificationRepository
{
    
}