using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindGoes.Sys
{
    /// <summary>
    /// Used to represent the result of a test.
    /// </summary>
    public class TestEventArgs: EventArgs
    {

        /// <summary>
        /// Used to represent the result of a test.
        /// </summary>
        /// <param name="result">Result of the test.</param>
        /// <param name="exception">Exception of the test.</param>
        public TestEventArgs(object result, Exception exception)
        {
            Result = result;
            Exception = exception;
        }

        /// <summary>
        /// Result of a test. 
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Exception if happened.
        /// </summary>
        public Exception Exception { get; set; }
    }
}
