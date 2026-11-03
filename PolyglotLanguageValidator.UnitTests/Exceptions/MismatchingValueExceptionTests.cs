using NUnit.Framework;
using PolyglotLanguageValidator.Exceptions;

namespace PolyglotLanguageValidator.UnitTests.Exceptions
{
    [TestFixture]
    public sealed class MismatchingValueExceptionTests
    {
        [TestCase("actual", "expected", "The actual value 'actual' does not match the expected value 'expected'.")]
        [TestCase("", "expected", "The actual value '' does not match the expected value 'expected'.")]
        [TestCase("actual", "", "The actual value 'actual' does not match the expected value ''.")]
        public void GivenActualAndExpectedValues_WhenConstructingTheException_ThenTheMessageDescribesTheMismatch(
            string actualValue,
            string expectedValue,
            string expectedMessage)
            => Assert.That(new MismatchingValueException(actualValue, expectedValue).Message, Is.EqualTo(expectedMessage));

        [Test]
        public void GivenNoValues_WhenConstructingTheException_ThenTheDefaultExceptionMessageIsUsed()
            => Assert.That(
                new MismatchingValueException().Message,
                Is.EqualTo("Exception of type 'PolyglotLanguageValidator.Exceptions.MismatchingValueException' was thrown."));
    }
}
