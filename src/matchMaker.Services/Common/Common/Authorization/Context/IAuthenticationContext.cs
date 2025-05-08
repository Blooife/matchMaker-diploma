namespace Common.Authorization.Context;

public interface IAuthenticationContext
{
    public long UserId { get; }

    public string IpAddress { get; }
    public bool IsUser { get; }
    public bool IsAdmin { get; }
    public bool IsModerator { get; }
}