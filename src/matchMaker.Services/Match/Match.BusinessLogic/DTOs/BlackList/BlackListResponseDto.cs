namespace Match.BusinessLogic.DTOs.BlackList;

public class BlackListResponseDto
{
    public string Id { get; set; }
    public long BlockerProfileId { get; set; }
    public long BlockedProfileId { get; set; }
    public string BlockedProfileFullName { get; set; }
    public DateTime CreatedAt { get; set; }
}