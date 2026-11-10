namespace LibraryApp.Common.Results
{
    public class GenericResult<T> : Result
    {
        public T Value { get; set; }
        public GenericResult(bool isSuccess, T value, string error = null) : base(isSuccess, error)
        {
            Value = value;
        }
        public static GenericResult<T> Success(T value) => new(true, value, string.Empty);
        public static GenericResult<T> Failure(string error) => new(false, default(T), error);
    }
}
