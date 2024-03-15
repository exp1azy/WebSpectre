namespace WebSpectre.Server.Exceptions
{
    public class EntityAlreadyExistException : Exception
    {
        public EntityAlreadyExistException() : base() { }
        public EntityAlreadyExistException(string message) : base(message) { }
        public EntityAlreadyExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}
