namespace Core.Exceptions;

public class ZwembadNotFoundException : Exception
{
    public ZwembadNotFoundException(string message) : base(message)
    {
    }

    public ZwembadNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
