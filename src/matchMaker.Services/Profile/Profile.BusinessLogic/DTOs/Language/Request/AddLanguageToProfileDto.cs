namespace Profile.BusinessLogic.DTOs.Language.Request;

public class AddLanguageToProfileDto
{
    public long ProfileId { get; set; }
    public long LanguageId { get; set; }
}