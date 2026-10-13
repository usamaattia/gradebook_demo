namespace Gradebook;

// One student and their three scores (each from 0 to 100).
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Homework { get; set; }
    public int Midterm { get; set; }
    public int Final { get; set; }
}
