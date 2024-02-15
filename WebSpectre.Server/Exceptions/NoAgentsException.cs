namespace WebSpectre.Server.Exceptions
{
    public class NoAgentsException : Exception
    {
        public NoAgentsException() : base() { }
        public NoAgentsException(string message) : base(message) { }
        public NoAgentsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
