namespace Core.Exceptions;

public class FeedbackNotFoundException : Exception
{
    public FeedbackNotFoundException(string message) : base(message)
    {
    }

    public FeedbackNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}