namespace Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(object key) : base($"Object (with id = {key}) was not found")
    {

    }
}