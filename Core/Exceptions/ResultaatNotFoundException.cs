namespace Core.Exceptions;

public class ResultaatNotFoundException : Exception
{
    public ResultaatNotFoundException(string message) : base(message)
    {
        
    }
    
    public ResultaatNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}