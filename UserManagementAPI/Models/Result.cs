namespace UserManagementAPI.Models
{
    /// <summary>
    /// Represents the outcome of an operation.
    /// On failure, the error reason will instead be provided.
    /// </summary>
    public class Result
    {
        /// <summary>Whether the operation succeeded.</summary>
        public bool IsSuccess { get; }

        /// <summary>Whether the operation failed.</summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>An error message describing what went wrong.</summary>
        public string Error { get; }

        protected Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>Creates a successful result.</summary>
        public static Result Success() => new(true, string.Empty);

        /// <summary>Creates a failed result with the given error message.</summary>
        public static Result Failure(string error) => new(false, error);
    }

    /// <summary>
    /// Represents the outcome of an operation that produces a value on success.
    /// On failure, the error reason will instead be provided.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>The value produced by a successful operation.</summary>
        public T? Value { get; }

        private Result(bool isSuccess, string error, T? value)
            : base(isSuccess, error)
        {
            Value = value;
        }

        /// <summary>Creates a successful result containing value object.</summary>
        public static Result<T> Success(T value) => new(true, string.Empty, value);

        /// <summary>Creates a failed result with the given error message.</summary>
        public new static Result<T> Failure(string error) => new(false, error, default);
    }
}