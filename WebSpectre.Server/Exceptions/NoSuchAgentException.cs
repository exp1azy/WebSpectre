namespace WebSpectre.Server.Exceptions
{
    public class NoSuchAgentException : Exception
    {
        public NoSuchAgentException() : base() { }
        public NoSuchAgentException(string message) : base(message) { }
        public NoSuchAgentException(string message, Exception innerException) : base(message, innerException) { }
    }
}
