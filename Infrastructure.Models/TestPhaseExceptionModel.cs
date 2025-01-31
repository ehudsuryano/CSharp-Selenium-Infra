using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    /// <summary>
    /// Describes a contract for receiving test exception data.
    /// </summary>
    /// <param name="exception">An <see cref="System.Exception"/> related to the <see cref="TestPhaseExceptionModel"/>.</param>
    public class TestPhaseExceptionModel(Exception exception)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestPhaseExceptionModel"/> class.
        /// </summary>
        public TestPhaseExceptionModel()
            : this(exception: default)
        { }

        /// <summary>
        /// Gets or sets the reference number for the attempt that caused the exception.
        /// </summary>
        public int AttemptReference { get; set; }

        /// <summary>
        /// Gets or sets the context dictionary for additional exception data.
        /// </summary>
        [JsonIgnore]
        public IDictionary<string, object> Context { get; set; }

        /// <summary>
        /// Gets or sets the display name for the exception.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Exception"/> object that was thrown.
        /// </summary>
        public Exception Exception { get; set; } = exception;

        /// <summary>
        /// Gets or sets the reason phrase for the exception model.
        /// </summary>
        public string ReasonPhrase { get; set; } = exception == default
            ? string.Empty
            : exception?.GetBaseException().Message;

        /// <summary>
        /// Gets or sets the screenshot in base64 format.
        /// </summary>
        [JsonIgnore, Newtonsoft.Json.JsonIgnore]
        public string Screenshot { get; set; }
    }
}
