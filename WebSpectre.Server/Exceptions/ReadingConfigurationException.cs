namespace WebSpectre.Server.Exceptions
{
    public class ReadingConfigurationException : Exception
    {
        public ReadingConfigurationException() : base() { }
        public ReadingConfigurationException(string message) : base(message) { }
        public ReadingConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
