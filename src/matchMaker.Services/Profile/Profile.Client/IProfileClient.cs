using Common.Dtos.Profile;

namespace Profile.Client;

public interface IProfileClient
{
    Task<ICollection<ProfileClientDto>> GetProfilesByIdsAsync(long[] profileIds);
}