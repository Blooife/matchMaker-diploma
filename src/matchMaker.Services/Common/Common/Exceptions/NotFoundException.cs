namespace Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(object key) : base($"Объект не найден")
    {

    }
}