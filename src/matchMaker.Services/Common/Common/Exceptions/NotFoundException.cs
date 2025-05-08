namespace Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string objectName, int sex = 1) : base(GetMessage(objectName, sex))
    {

    }
    
    private static string GetMessage(string objectName, int sex)
    {
        string ending = sex switch
        {
            2 => "а",
            3 => "ы",
            4 => "о",
            _ => string.Empty,
        };
            
        return $"{objectName}{ending} не найден";
    }
}