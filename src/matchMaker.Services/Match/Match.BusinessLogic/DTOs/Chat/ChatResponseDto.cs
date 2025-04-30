namespace Match.BusinessLogic.DTOs.Chat;

public class ChatResponseDto
{
    public string Id { get; set; }
    public string ProfileName { get; set; }
    public string ProfileLastName { get; set; }
    public string MainImageUrl { get; set; }
    public long FirstProfileId { get; set; }
    public long SecondProfileId { get; set; }
    public long RequestedProfileUnreadCount { get; set; }
    public long ReceiverProfileUnreadCount { get; set; }
}