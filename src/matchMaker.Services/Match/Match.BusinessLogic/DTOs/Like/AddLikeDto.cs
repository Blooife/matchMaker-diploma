namespace Match.BusinessLogic.DTOs.Like;

public class AddLikeDto
{
    public long ProfileId { get; set; }
    public long TargetProfileId { get; set; }
    public bool IsLike { get; set; }
}