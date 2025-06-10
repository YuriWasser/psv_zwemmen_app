namespace Core.Exceptions;

public class TrainingNotFoundException : Exception
{
    public TrainingNotFoundException(string message) : base(message)
    {
        
    }
    
    public TrainingNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}