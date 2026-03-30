namespace UserManagementAPI.Models
{
    /// <summary>
    /// Represents the outcome of an operation.
    /// On failure, validation errors will be provided.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Indicates whether the operation failed.
        /// </summary>
        public bool IsFailure { get; set; }

        /// <summary>
        /// A dictionary of validation errors, where the key is the field name
        /// and the value is an array of error messages for that field.
        /// Null if the operation was successful.
        /// </summary>
        public Dictionary<string, string[]>? Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of the Result class.
        /// </summary>
        /// <param name="isFailure">Whether the operation failed.</param>
        /// <param name="errors">Validation errors, if any.</param>
        protected Result(bool isFailure, Dictionary<string, string[]>? errors)
        {
            IsFailure = isFailure;
            Errors = errors;
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        public static Result Success() => new(false, null);

        /// <summary>
        /// Creates a failed result with the given validation errors.
        /// </summary>
        /// <param name="errors">A dictionary of validation errors.</param>
        public static Result Failure(Dictionary<string, string[]> errors)
            => new(true, errors);
    }

    /// <summary>
    /// Represents the outcome of an operation that produces a value on success.
    /// On failure, validation errors will be provided.
    /// </summary>
    /// <typeparam name="T">The type of the value produced on success.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// The value produced by a successful operation.
        /// Null if the operation failed.
        /// </summary>
        public T? Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="isFailure">Whether the operation failed.</param>
        /// <param name="errors">Validation errors, if any.</param>
        /// <param name="value">The value produced by the operation, if successful.</param>
        private Result(bool isFailure, Dictionary<string, string[]>? errors, T? value)
            : base(isFailure, errors)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a successful result containing the specified value.
        /// </summary>
        /// <param name="value">The value produced by the operation.</param>
        public static Result<T> Success(T value) => new(false, null, value);

        /// <summary>
        /// Creates a failed result with the given validation errors.
        /// </summary>
        /// <param name="errors">A dictionary of validation errors.</param>
        public new static Result<T> Failure(Dictionary<string, string[]> errors) => new(true, errors, default);
    }
}