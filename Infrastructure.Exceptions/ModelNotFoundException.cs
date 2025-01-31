namespace Infrastructure.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when a model is not found.
    /// </summary>
    public class ModelNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNotFoundException"/> class.
        /// </summary>
        
        public ModelNotFoundException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNotFoundException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        
        public ModelNotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNotFoundException"/> class with a specified error
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        
        public ModelNotFoundException (string message,Exception innerException) : base(message, innerException) { }

    }
}
