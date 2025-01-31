using System;

namespace Infrastructure.Models
{
    /// <summary>
    /// Represents the results of a specific phase in a test case.
    /// </summary>
    public class TestPhaseResultModel
    {
        /// <summary>
        /// Gets or sets the duration of the test phase.
        /// </summary>
        /// <value>The duration of the test phase as a <see cref="TimeSpan"/>.</value>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the end time of the test phase.
        /// </summary>
        /// <value>The end time of the test phase as a <see cref="DateTime"/>.</value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the exception details if the test phase encountered an error.
        /// </summary>
        /// <value>The exception details as a <see cref="TestPhaseExceptionModel"/>.</value>
        public TestPhaseExceptionModel PhaseException { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test phase was successful.
        /// </summary>
        /// <value><c>true</c> if the test phase was successful; otherwise, <c>false</c>.</value>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the name of the test phase.
        /// </summary>
        /// <value>The name of the test phase.</value>
        public string PhaseName { get; set; }

        /// <summary>
        /// Gets or sets the start time of the test phase.
        /// </summary>
        /// <value>The start time of the test phase as a <see cref="DateTime"/>.</value>
        public DateTime StartTime { get; set; }
    }
}
