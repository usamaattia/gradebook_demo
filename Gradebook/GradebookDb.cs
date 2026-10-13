using Microsoft.EntityFrameworkCore;

namespace Gradebook;

// The database. Each DbSet is a table; the Student class describes its columns.
public class GradebookDb : DbContext
{
    public DbSet<Student> Students { get; set; }

    // Store everything in one SQLite file next to the app.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=gradebook.db");
    }

    // Creates the database and, if it is empty, fills it with sample students.
    public static void Setup()
    {
        using var db = new GradebookDb();

        // Create the database file and the Students table if they don't exist yet.
        db.Database.EnsureCreated();

        // Only add sample data when the table is empty.
        if (!db.Students.Any())
        {
            db.Students.Add(new Student { Name = "Omar", Homework = 88, Midterm = 88, Final = 93 });
            db.Students.Add(new Student { Name = "Sara", Homework = 85, Midterm = 72, Final = 91 });
            db.Students.Add(new Student { Name = "Lina", Homework = 70, Midterm = 65, Final = 80 });
            db.Students.Add(new Student { Name = "Yousef", Homework = 60, Midterm = 75, Final = 58 });
            db.Students.Add(new Student { Name = "Mona", Homework = 50, Midterm = 45, Final = 55 });
            db.SaveChanges();
        }
    }
}
