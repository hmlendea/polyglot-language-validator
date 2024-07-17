using System;

namespace PolyglotTester.Exceptions
{
    public class MismatchingValueException : Exception
    {
        static string ExceptionMessageFormat => "The actual value '{0}' does not match the expected value '{1}'.";

        public MismatchingValueException() : base() { }

        public MismatchingValueException(string actualValue, string expectedValue)
        : base(string.Format(ExceptionMessageFormat, actualValue, expectedValue)) { }

        public MismatchingValueException(string actualValue, string expectedValue, Exception innerException)
        : base(string.Format(ExceptionMessageFormat, actualValue, expectedValue), innerException) { }
    }
}
