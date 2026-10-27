using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{
    [TestCase(17, 2, true, "0.0", ExpectedResult = true, TestName = "WhenDecimal_True")]
    [TestCase(17, 2, true, "0", ExpectedResult = true, TestName = "WhenInteger_True")]
    [TestCase(3, 0, true, "+10", ExpectedResult = true, TestName = "WhenPlusSign_True")]
    [TestCase(3, 2, true, "+1.23", ExpectedResult = false, TestName = "WhenNotEnoughPrecision_False")]
    [TestCase(17, 2, true, "0.000", ExpectedResult = false, TestName = "WhenNotEnoughScale_False")]
    [TestCase(3, 2, true, "-0", ExpectedResult = false, TestName = "WhenViolatesNotPositive_False")]
    [TestCase(17, 2, false, "-0", ExpectedResult = true, TestName = "WhenNotViolatesNotPositive_True")]
    [TestCase(7, 3, false, "-430.313", ExpectedResult = true, TestName = "WhenEnoughPrecisionAndScale_True")]
    [TestCase(17, 2, false, "a.sd", ExpectedResult = false, TestName = "WhenInvalidValue_False")]
    [TestCase(17, 2, false, "1,11", ExpectedResult = true, TestName = "WhenComma_True")]
    public bool IsValidNumber(int precision, int scale, bool onlyPositive, string test)
    {
        return new NumberValidator(precision, scale, onlyPositive).IsValidNumber(test);
    }

    [TestCase(3, -2, true, TestName = "WhenScaleNegative_ArgumentException")]
    [TestCase(-1, 2, true, TestName = "WhenPrecisionNegative_ArgumentException")]
    [TestCase(2, 3, true, TestName = "WhenScaleBiggerThenPrecision_ArgumentException")]
    [TestCase(2, 2, true, TestName = "WhenPrecisionEqualsScale_ArgumentException")]
    [TestCase(1, 0, false, TestName = "WhenScaleEqualsZero_WorksCorrectly")]
    [TestCase(0, 0, true, TestName = "WhenPrecisionEqualsZero_ArgumentException")]
    [TestCase(7, 2, false, TestName = "WhenValidScaleAndPrecision_WorksCorrectly")]
    public void Constructor(int precision, int scale, bool shouldThrow)
    {
        Action action = () => new NumberValidator(precision, scale, true);
        if (shouldThrow)
            action.Should().Throw<ArgumentException>();
        else action.Should().NotThrow();
    }
}