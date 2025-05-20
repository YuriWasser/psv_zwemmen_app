namespace Core.Exceptions;

public class GebruikerNotFoundException : Exception
{
    public GebruikerNotFoundException(string message) : base(message)
    {
        
    }
    
    public GebruikerNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}