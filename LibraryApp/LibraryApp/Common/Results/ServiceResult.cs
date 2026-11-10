namespace LibraryApp.Common.Results
{
    public enum ServiceErrorType
    {
        NotFound,
        Conflict
    }

    public sealed class ServiceResult<T> where T : class
    {
        private ServiceResult(T? value, ServiceErrorType? errorType, string? error)
        {
            Value = value;
            ErrorType = errorType;
            Error = error;
        }

        public T? Value { get; }
        public ServiceErrorType? ErrorType { get; }
        public string? Error { get; }
        public bool IsSuccess => ErrorType is null;

        public static ServiceResult<T> Success(T value) => new(value, null, null);

        public static ServiceResult<T> Failure(ServiceErrorType errorType, string error) =>
            new(null, errorType, error);
    }
}
