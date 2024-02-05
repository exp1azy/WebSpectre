namespace WebSpectre.Server.Exceptions
{
    public class WebSpectreRedisConnectionException : Exception
    {
        public WebSpectreRedisConnectionException() : base() { }
        public WebSpectreRedisConnectionException(string message) : base(message) { }
        public WebSpectreRedisConnectionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
