namespace Match.BusinessLogic.DTOs.Chat;

public class ChatResponseDto
{
    public long Id { get; set; }
    public string ProfileName { get; set; }
    public string ProfileLastName { get; set; }
    public string MainImageUrl { get; set; }
    public long FirstProfileId { get; set; }
    public long SecondProfileId { get; set; }
}