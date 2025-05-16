namespace Core.Exceptions;

public class CompetitieNotFoundException : Exception
{
    public CompetitieNotFoundException(string message) : base(message)
    {
    }

    public CompetitieNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}