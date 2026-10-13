namespace Gradebook.Tests;

// Tests for GradeCalculator. Every expected value was worked out by hand.
public class GradeCalculatorTests
{
    // Average: homework 30%, midterm 30%, final 40%, rounded to 1 decimal.
    [Theory]
    [InlineData(85, 72, 91, 83.5)]
    [InlineData(100, 100, 100, 100)]
    [InlineData(0, 0, 0, 0)]
    [InlineData(88, 88, 93, 90)]
    public void Average_ReturnsWeightedAverage(int homework, int midterm, int final, double expected)
    {
        double result = GradeCalculator.Average(homework, midterm, final);

        Assert.Equal(expected, result);
    }

    // Letter: exactly on the line counts as the higher grade.
    [Theory]
    [InlineData(95, "A")]
    [InlineData(90, "A")]
    [InlineData(89.9, "B")]
    [InlineData(80, "B")]
    [InlineData(79.9, "C")]
    [InlineData(70, "C")]
    [InlineData(60, "D")]
    [InlineData(59.9, "F")]
    [InlineData(0, "F")]
    public void Letter_ReturnsCorrectGrade(double average, string expected)
    {
        string result = GradeCalculator.Letter(average);

        Assert.Equal(expected, result);
    }

    // A score must be between 0 and 100 (both included).
    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public void IsValidScore_ChecksRange(int score, bool expected)
    {
        bool result = GradeCalculator.IsValidScore(score);

        Assert.Equal(expected, result);
    }

    // Both together: 88, 88, 93 averages exactly 90, which is an A.
    [Fact]
    public void Student_With_88_88_93_GetsA()
    {
        double average = GradeCalculator.Average(88, 88, 93);

        string letter = GradeCalculator.Letter(average);

        Assert.Equal("A", letter);
    }
}
