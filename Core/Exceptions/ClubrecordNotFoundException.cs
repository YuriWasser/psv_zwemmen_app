namespace Core.Exceptions;

public class ClubrecordNotFoundException : Exception
{
    public ClubrecordNotFoundException(string message) : base(message)
    {
        
    }
    
    public ClubrecordNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}