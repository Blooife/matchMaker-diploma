namespace Profile.DataAccess.Models;

public class Image : BaseModel<long>
{
    public string ImageUrl { get; set; }
    public long ProfileId { get; set; }
    public bool IsMainImage { get; set; }
    public DateTime UploadTimestamp { get; set; }
    public UserProfile Profile { get; set; }
}