namespace Core.Exceptions;

public class AfstandPerProgrammaNotFound : Exception
{
    public AfstandPerProgrammaNotFound (string message) : base(message)
    {
    }
    
    public AfstandPerProgrammaNotFound(string message, Exception innerException) : base(message, innerException)
    {
    }
}