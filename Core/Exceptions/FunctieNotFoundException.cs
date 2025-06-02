namespace Core.Exceptions;

public class FunctieNotFoundException : Exception
{
    public FunctieNotFoundException(string message) : base(message)
    {
        
    }
    
    public FunctieNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}