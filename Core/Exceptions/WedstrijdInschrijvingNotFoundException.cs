namespace Core.Exceptions;

public class WedstrijdInschrijvingNotFoundException : Exception
{
    public WedstrijdInschrijvingNotFoundException(string message) : base(message)
    {
        
    }

    public WedstrijdInschrijvingNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}