using Common.Constants;
using MessageQueue.MessageContracts;

namespace MessageQueue.Messages.Profile;

public class ProfileCreatedEventMessage : IAsyncCommunicationEvent
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public int AgeFrom { get; set; }
    public int AgeTo { get; set; }
    public Gender Gender { get; set; }
    public Gender PreferredGender { get; set; }
    public int MaxDistance { get; set; }
    public long CountryId { get; set; }
    public string Country { get; set; }
    public long CityId { get; set; }
    public string City { get; set; }
    public string MainImageUrl { get; set; }
    public string RoutingKey { get; set; } = "profile.created";
}