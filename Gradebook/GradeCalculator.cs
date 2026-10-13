namespace Gradebook;

// The grading rules. Pure logic: no database, no web page, so it is easy to test.
public static class GradeCalculator
{
    // Weighted average: homework 30%, midterm 30%, final 40%, rounded to 1 decimal.
    public static double Average(int homework, int midterm, int final)
    {
        double average = homework * 0.30 + midterm * 0.30 + final * 0.40;
        return Math.Round(average, 1);
    }

    // Turns an average into a letter grade.
    public static string Letter(double average)
    {
        if (average >= 90) return "A";
        if (average >= 80) return "B";
        if (average >= 70) return "C";
        if (average >= 60) return "D";
        return "F";
    }

    // A score is valid only if it is between 0 and 100 (both included).
    public static bool IsValidScore(int score)
    {
        return score >= 0 && score <= 100;
    }
}
