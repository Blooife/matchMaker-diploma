namespace Common.Exceptions;

public class DeleteUserException : Exception
{
    public DeleteUserException(string message) : base(message)
    {
    }
}