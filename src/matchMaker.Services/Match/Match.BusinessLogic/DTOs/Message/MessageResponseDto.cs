namespace Match.BusinessLogic.DTOs.Message;

public class MessageResponseDto
{
    public string Id { get; set; }
    public long SenderId { get; set; }
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
    public string ChatId { get; set; }
}