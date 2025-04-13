using Common.Dtos.Profile;
using Common.Http;
using Microsoft.AspNetCore.Http;

namespace Profile.Client;

public class ProfileClient(
    IHttpClientFactory httpFactory,
    IHttpContextAccessor httpContextAccessor,
    string baseUrl)
    : BaseHttpClient(httpFactory, httpContextAccessor, baseUrl), IProfileClient
{
    public Task<ICollection<ProfileClientDto>> GetProfilesByIdsAsync(long[] profileIds)
    {
        var url = "profiles/by-ids/get";
        
        return PostData<long[], ICollection<ProfileClientDto>>(profileIds, url);
    }
}