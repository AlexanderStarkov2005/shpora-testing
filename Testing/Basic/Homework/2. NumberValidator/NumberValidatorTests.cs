using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{

    [TestCase(3, 2, true, "+1.23", TestName = "WhenNotEnoughPrecision")]
    [TestCase(17, 2, true, "0.000", TestName = "WhenNotEnoughScale")]
    [TestCase(3, 2, true, "-0", TestName = "WhenViolatesNotPositive")]
    [TestCase(17, 2, false, "a.sd", TestName = "WhenInvalidValue")]
    public void IsValidNumber_ShouldBeFalse(int precision, int scale, bool onlyPositive, string test)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(test).Should().BeFalse();
    }
    
    [TestCase(17, 2, true, "0.0", TestName = "WhenDecimal")]
    [TestCase(17, 2, true, "0", TestName = "WhenInteger")]
    [TestCase(3, 0, true, "+10", TestName = "WhenPlusSign")]
    [TestCase(17, 2, false, "-0", TestName = "WhenNotViolatesNotPositive")]
    [TestCase(7, 3, false, "-430.313", TestName = "WhenEnoughPrecisionAndScale")]
    [TestCase(17, 2, false, "1,11", TestName = "WhenComma")]
    public void IsValidNumber_ShouldBeTrue(int precision, int scale, bool onlyPositive, string test)
    {
        new NumberValidator(precision, scale, onlyPositive).IsValidNumber(test).Should().BeTrue();
    }
    
    [TestCase(1, 0, TestName = "WhenScaleEqualsZero")]
    public void Constructor_WorksCorrectly(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale, true);
        action.Should().NotThrow();
    }
    
    [TestCase(3, -2, TestName = "WhenScaleNegative")]
    [TestCase(2, 3, TestName = "WhenScaleBiggerThenPrecision")]
    [TestCase(2, 2, TestName = "WhenScaleEqualsPrecision")]
    public void Constructor_ThrowsExceptionAboutScale(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale, true);
        
        //В исходном классе опечатка, должно быть scale must be a non-negative number less than precision
        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a non-negative number less or equal than precision");
    }
    
    [TestCase(-1, 2, TestName = "WhenPrecisionNegative")]
    [TestCase(0, 0, TestName = "WhenPrecisionEqualsZero")]
    public void Constructor_ThrowsExceptionAboutPrecision(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }
}