using FluentAssertions;
using NUnit.Framework;

namespace HomeExercise.Tasks.NumberValidator;

[TestFixture]
public class NumberValidatorTests
{

    [TestCase(3, 2, true, "+1.23", TestName = "WhenNotEnoughPrecision")]
    [TestCase(17, 2, true, "0.000", TestName = "WhenNotEnoughScale")]
    [TestCase(3, 2, true, "-0", TestName = "WhenViolatesNotPositive")]
    [TestCase(3, 2, true, "", TestName = "WhenEmptyString")]
    [TestCase(3, 2, true, null, TestName = "WhenNull")]
    [TestCase(3, 2, true, "1..2", TestName = "WhenDoubleDot")]
    [TestCase(3, 2, true, "1.2.3", TestName = "WhenMultipleDots")]
    [TestCase(3, 2, true, ".5", TestName = "WhenNoIntegerPart")]
    [TestCase(3, 2, true, "1.", TestName = "WhenDotWithoutFraction")]
    [TestCase(3, 2, true, "1,2,3", TestName = "WhenMultipleCommas")]
    [TestCase(3, 2, true, "1.2,3", TestName = "WhenMixedSeparators")]
    [TestCase(3, 2, true, "--1.2", TestName = "WhenDoubleMinus")]
    [TestCase(3, 2, true, "+-1.2", TestName = "WhenPlusMinus")]
    [TestCase(3, 2, true, "1-2", TestName = "WhenMinusInMiddle")]
    [TestCase(3, 2, true, "abc", TestName = "WhenLettersOnly")]
    [TestCase(3, 2, true, "1.2abc", TestName = "WhenTrailingLetters")]
    [TestCase(3, 2, true, "1e5", TestName = "WhenExponentialNotation")]
    [TestCase(3, 2, true, "0x1F", TestName = "WhenHexNotation")]
    public void IsValidNumber_ShouldBeFalse(int precision, int scale, bool onlyPositive, string test)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        var result = validator.IsValidNumber(test);
        
        result.Should().BeFalse();
    }
    
    [TestCase(17, 2, true, "00.00", TestName = "WhenLeadingZeros")]
    [TestCase(17, 2, true, "0.0", TestName = "WhenDecimal")]
    [TestCase(17, 2, true, "0", TestName = "WhenInteger")]
    [TestCase(3, 0, true, "+10", TestName = "WhenPlusSign")]
    [TestCase(17, 2, false, "-0", TestName = "WhenNotViolatesNotPositive")]
    [TestCase(7, 3, false, "-430.313", TestName = "WhenEnoughPrecisionAndScale")]
    [TestCase(17, 2, false, "1,11", TestName = "WhenComma")]
    [TestCase(7, 3, true, "+430.313", TestName = "WhenPlusButNegativeNotAllowed")]
    public void IsValidNumber_ShouldBeTrue(int precision, int scale, bool onlyPositive, string test)
    {
        var validator = new NumberValidator(precision, scale, onlyPositive);
        
        var result = validator.IsValidNumber(test);
        
        result.Should().BeTrue();
    }
    
    [TestCase(1, 0, TestName = "WhenMinimalValid")]
    [TestCase(100, 99, TestName = "WhenLargePrecisionScaleLessByOne")]
    [TestCase(5, 0, TestName = "WhenScaleZeroPrecisionFive")]
    [TestCase(int.MaxValue, 0, TestName = "WhenPrecisionMaxInt")]
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
    [TestCase(-1, -1, TestName = "WhenBothInvalidPrecisionPriority")]
    public void Constructor_ThrowsExceptionAboutPrecision(int precision, int scale)
    {
        Action action = () => new NumberValidator(precision, scale, true);
        
        action.Should().Throw<ArgumentException>()
            .WithMessage("precision must be a positive number");
    }
}