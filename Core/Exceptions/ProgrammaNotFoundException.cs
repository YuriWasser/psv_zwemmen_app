namespace Core.Exceptions;

public class ProgrammaNotFoundException : Exception
{
    public ProgrammaNotFoundException(string message) : base(message)
    {
    }
    
    public ProgrammaNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}