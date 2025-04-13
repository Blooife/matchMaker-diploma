namespace Profile.BusinessLogic.DTOs.Language.Request;

public class RemoveLanguageFromProfileDto
{
    public string ProfileId { get; set; }
    public int LanguageId { get; set; }
}