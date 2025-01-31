using System;
using System.Collections.Generic;

namespace Infrastructure.Models
{
    /// <summary>
    /// Represents the results of a test case.
    /// </summary>
    public class TestResultModel
    {
        /// <summary>
        /// Gets or sets the current attempt number for executing the test case.
        /// </summary>
        /// <value>The current attempt number.</value>
        public int AttemptNumber { get; set; }

        /// <summary>
        /// Gets or sets a dictionary of auditable actions and their corresponding metrics.
        /// </summary>
        /// <value>A dictionary where the key is the action name and the value is the metric associated with that action.</value>
        public Dictionary<string, object> AuditableActions { get; set; }

        /// <summary>
        /// Gets or sets the display name of the test case.
        /// </summary>
        /// <value>The display name of the test case.</value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the collection of exceptions that occurred during the test case.
        /// </summary>
        /// <value>A collection of <see cref="TestPhaseExceptionModel"/> instances.</value>
        public IEnumerable<TestPhaseExceptionModel> Exceptions { get; set; }

        /// <summary>
        /// Gets or sets a dictionary of metrics related to the test case.
        /// </summary>
        /// <value>A dictionary where the key is the metric name and the value is the metric value.</value>
        public Dictionary<string, object> Metrics { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test case passed.
        /// </summary>
        /// <value><c>true</c> if the test case passed; otherwise, <c>false</c>.</value>
        public bool Passed { get; set; }

        /// <summary>
        /// Gets or sets the time taken to find elements during the test case.
        /// </summary>
        /// <value>The find elements duration as a <see cref="TimeSpan"/>.</value>
        public TimeSpan FindElementsDuration { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the current test run.
        /// </summary>
        /// <value>The unique identifier for the test run.</value>
        public string RunId { get; set; }

        /// <summary>
        /// Gets or sets the time taken to set up the test case.
        /// </summary>
        /// <value>The setup time as a <see cref="TimeSpan"/>.</value>
        public TimeSpan SetupDuration { get; set; }

        /// <summary>
        /// Gets or sets the end time of the setup phase.
        /// </summary>
        /// <value>The setup end time as a <see cref="DateTime"/>.</value>
        public DateTime SetupEndTime { get; set; }

        /// <summary>
        /// Gets or sets the start time of the setup phase.
        /// </summary>
        /// <value>The setup start time as a <see cref="DateTime"/>.</value>
        public DateTime SetupStartTime { get; set; }

        /// <summary>
        /// Gets or sets the time taken to tear down the test case.
        /// </summary>
        /// <value>The teardown time as a <see cref="TimeSpan"/>.</value>
        public TimeSpan TeardownDuration { get; set; }

        /// <summary>
        /// Gets or sets the end time of the teardown phase.
        /// </summary>
        /// <value>The teardown end time as a <see cref="DateTime"/>.</value>
        public DateTime TeardownEndTime { get; set; }

        /// <summary>
        /// Gets or sets the start time of the teardown phase.
        /// </summary>
        /// <value>The teardown start time as a <see cref="DateTime"/>.</value>
        public DateTime TeardownStartTime { get; set; }

        /// <summary>
        /// Gets or sets the name of the test class.
        /// </summary>
        /// <value>The name of the test class.</value>
        public string TestClassName { get; set; }

        /// <summary>
        /// Gets or sets the end time of the test execution phase.
        /// </summary>
        /// <value>The test end time as a <see cref="DateTime"/>.</value>
        public DateTime TestEndTime { get; set; }

        /// <summary>
        /// Gets or sets the name of the test method.
        /// </summary>
        /// <value>The name of the test method.</value>
        public string TestMethodName { get; set; }

        /// <summary>
        /// Gets or sets the start time of the test execution phase.
        /// </summary>
        /// <value>The test start time as a <see cref="DateTime"/>.</value>
        public DateTime TestStartTime { get; set; }

        /// <summary>
        /// Gets or sets the time taken to execute the test case.
        /// </summary>
        /// <value>The test time as a <see cref="TimeSpan"/>.</value>
        public TimeSpan TestDuration { get; set; }
    }
}
