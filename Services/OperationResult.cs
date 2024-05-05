namespace server.Services
{
    public class OperationResult
    {
        public bool Success { get; }
        public string Message { get; }

        public OperationResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
