namespace Profile.BusinessLogic.DTOs.Image.Response;

public class ImageResponseDto
{
    public long Id { get; set; }
    public string ImageUrl { get; set; }
    public string ProfileId { get; set; }
    public bool IsMainImage { get; set; }
}