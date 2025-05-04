namespace Common.Exceptions;

public static class ExceptionMessages
{
    public const string ErrorAssigningRole = "Возникла ошибка в процессе назначения роли";
    public const string ErrorRemovingRole = "Возникла ошибка в процессе удаления роли";
    public const string LoginFailed = "Логин иили пароль введены неправильно";
    public const string RoleNotExists = "Роль не найдена";
    public const string DeleteUserFailed = "Возникла ошибка во время удаления пользователя";
    public const string ProfileContainsLanguage = "В профиль уже добавлен этот язык";
    public const string ProfileContainsInterest = "В профиль уже добавлен этот интерес";
    public const string ProfileContainsEducation = "В профиль уже добавлен это образование";
    
    public const string ProfileNotContainsLanguage = "В профиль ещё не добавлен этот язык";
    public const string ProfileNotContainsInterest = "В профиль ещё не добавлен этот интерес";
    public const string ProfileNotContainsEducation = "PВ профиль ещё не добавлен это образование";
}