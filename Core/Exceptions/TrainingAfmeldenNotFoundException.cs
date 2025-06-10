namespace Core.Exceptions;

public class TrainingAfmeldenNotFoundException : Exception
{
    public TrainingAfmeldenNotFoundException(string message) : base(message)
    {
        
    }
    
    public TrainingAfmeldenNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}
