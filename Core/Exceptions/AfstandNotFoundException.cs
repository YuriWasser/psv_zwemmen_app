namespace Core.Exceptions;

public class AfstandNotFoundException : Exception
{
    public AfstandNotFoundException(string message) : base(message)
    {
    }

    public AfstandNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
