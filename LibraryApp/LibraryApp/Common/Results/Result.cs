namespace LibraryApp.Common.Results
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; set; }
        public Result(bool isSuccess, string error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }
        public static Result Success() => new(true);
        public static Result Failure(string error) => new(false, error);
    }
}
