using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// Exception that is thrown when there is an error during test setup.
    /// </summary>
    public class TestSetupException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSetupException"/> class.
        /// </summary>
        public TestSetupException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSetupException"/> class with a
        /// specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>

        public TestSetupException(string message) :base(message){ }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSetupException"/> class with a
        /// specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
         
        public TestSetupException(string message , Exception innerException) : base(message, innerException) { }

    }


}
