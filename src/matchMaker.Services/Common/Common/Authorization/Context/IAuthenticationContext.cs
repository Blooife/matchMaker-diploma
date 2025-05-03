namespace Common.Authorization.Context;

public interface IAuthenticationContext
{
    public long UserId { get; }

    public string IpAddress { get; }
}