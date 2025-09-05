using System;
using System.Collections.Generic;
using System.Linq;

// ================== Models ==================
public class Student
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public double GPA { get; set; }

    public override string ToString() => $"Student: {Id} | {Name} | Age:{Age} | GPA:{GPA}";
}

public class Teacher
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Subject { get; set; }

    public override string ToString() => $"Teacher: {Id} | {Name} | Subject:{Subject}";
}

public class Course
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string TeacherId { get; set; }

    public override string ToString() => $"Course: {Id} | {Name} | TeacherId:{TeacherId}";
}

public class Enrollment
{
    public string StudentId { get; set; }
    public string CourseId { get; set; }

    public override string ToString() => $"Enrollment: Student {StudentId} -> Course {CourseId}";
}

public class Grade
{
    public string StudentId { get; set; }
    public string CourseId { get; set; }
    public double Score { get; set; }

    public override string ToString() => $"Grade: Student {StudentId} | Course {CourseId} | Score:{Score}";
}

// ================== Repositories ==================
public class Repository<T>
{
    protected List<T> items = new List<T>();

    public void Add(T item) => items.Add(item);
    public void Remove(Func<T, bool> predicate) => items.RemoveAll(x => predicate(x));
    public List<T> GetAll() => items;
    public T Find(Func<T, bool> predicate) => items.Find(predicate);
    public List<T> FindAll(Func<T, bool> predicate) => items.FindAll(predicate);
}

public class StudentRepository : Repository<Student>
{
    public void SortByName() => items.Sort((a, b) => a.Name.CompareTo(b.Name));
    public void SortByGPA() => items.Sort((a, b) => b.GPA.CompareTo(a.GPA));
}

public class TeacherRepository : Repository<Teacher> { }
public class CourseRepository : Repository<Course> { }
public class EnrollmentRepository : Repository<Enrollment> { }
public class GradeRepository : Repository<Grade> { }

// ================== Services ==================
public class ReportService
{
    private StudentRepository students;
    private TeacherRepository teachers;
    private CourseRepository courses;
    private EnrollmentRepository enrollments;
    private GradeRepository grades;

    public ReportService(StudentRepository s, TeacherRepository t, CourseRepository c,
                         EnrollmentRepository e, GradeRepository g)
    {
        students = s;
        teachers = t;
        courses = c;
        enrollments = e;
        grades = g;
    }

    // Báo cáo: Liệt kê SV theo môn học
    public void ReportStudentsByCourse(string courseId)
    {
        var enrolled = enrollments.FindAll(e => e.CourseId == courseId);
        Console.WriteLine($"--- Students in Course {courseId} ---");
        foreach (var e in enrolled)
        {
            var st = students.Find(s => s.Id == e.StudentId);
            Console.WriteLine(st != null ? st.ToString() : $"Unknown Student {e.StudentId}");
        }
    }

    // Báo cáo: Điểm của SV theo môn học
    public void ReportGradesByCourse(string courseId)
    {
        var gList = grades.FindAll(g => g.CourseId == courseId);
        Console.WriteLine($"--- Grades in Course {courseId} ---");
        foreach (var g in gList)
        {
            var st = students.Find(s => s.Id == g.StudentId);
            Console.WriteLine(st != null ? $"{st.Name}: {g.Score}" : $"Unknown Student {g.StudentId}");
        }
    }

    // Báo cáo: GPA trung bình toàn trường
    public void ReportAverageGPA()
    {
        if (students.GetAll().Count == 0) { Console.WriteLine("No students."); return; }
        double avg = students.GetAll().Average(s => s.GPA);
        Console.WriteLine($"Average GPA of all students: {avg:F2}");
    }
}

// ================== UI (Program) ==================
public class Program
{
    static void Main(string[] args)
    {
        var students = new StudentRepository();
        var teachers = new TeacherRepository();
        var courses = new CourseRepository();
        var enrollments = new EnrollmentRepository();
        var grades = new GradeRepository();
        var report = new ReportService(students, teachers, courses, enrollments, grades);

        int menu = 0;
        while (menu != 99)
        {
            Console.WriteLine("============= MAIN MENU =============");
            Console.WriteLine("1. Student Management");
            Console.WriteLine("2. Teacher Management");
            Console.WriteLine("3. Course Management");
            Console.WriteLine("4. Enrollment Management");
            Console.WriteLine("5. Grade Management");
            Console.WriteLine("6. Reports");
            Console.WriteLine("99. Exit");
            Console.Write("Choice: ");
            menu = int.Parse(Console.ReadLine());

            switch (menu)
            {
                case 1:
                    ManageStudents(students);
                    break;
                case 2:
                    ManageTeachers(teachers);
                    break;
                case 3:
                    ManageCourses(courses);
                    break;
                case 4:
                    ManageEnrollments(enrollments);
                    break;
                case 5:
                    ManageGrades(grades);
                    break;
                case 6:
                    ManageReports(report);
                    break;
            }
        }
    }

    // =========== Submenus ===========
    static void ManageStudents(StudentRepository repo)
    {
        Console.WriteLine("--- Student Management ---");
        Console.WriteLine("1. Add | 2. Show All | 3. Sort by GPA | 4. Sort by Name");
        int choice = int.Parse(Console.ReadLine());
        if (choice == 1)
        {
            Console.Write("ID: "); string id = Console.ReadLine();
            Console.Write("Name: "); string name = Console.ReadLine();
            Console.Write("Age: "); int age = int.Parse(Console.ReadLine());
            Console.Write("GPA: "); double gpa = double.Parse(Console.ReadLine());
            repo.Add(new Student { Id = id, Name = name, Age = age, GPA = gpa });
        }
        else if (choice == 2)
        {
            foreach (var s in repo.GetAll()) Console.WriteLine(s);
        }
        else if (choice == 3)
        {
            repo.SortByGPA(); Console.WriteLine("Sorted by GPA.");
        }
        else if (choice == 4)
        {
            repo.SortByName(); Console.WriteLine("Sorted by Name.");
        }
    }

    static void ManageTeachers(TeacherRepository repo)
    {
        Console.WriteLine("--- Teacher Management ---");
        Console.Write("ID: "); string id = Console.ReadLine();
        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("Subject: "); string sub = Console.ReadLine();
        repo.Add(new Teacher { Id = id, Name = name, Subject = sub });
        foreach (var t in repo.GetAll()) Console.WriteLine(t);
    }

    static void ManageCourses(CourseRepository repo)
    {
        Console.WriteLine("--- Course Management ---");
        Console.Write("ID: "); string id = Console.ReadLine();
        Console.Write("Name: "); string name = Console.ReadLine();
        Console.Write("TeacherId: "); string tid = Console.ReadLine();
        repo.Add(new Course { Id = id, Name = name, TeacherId = tid });
        foreach (var c in repo.GetAll()) Console.WriteLine(c);
    }

    static void ManageEnrollments(EnrollmentRepository repo)
    {
        Console.WriteLine("--- Enrollment Management ---");
        Console.Write("StudentId: "); string sid = Console.ReadLine();
        Console.Write("CourseId: "); string cid = Console.ReadLine();
        repo.Add(new Enrollment { StudentId = sid, CourseId = cid });
        foreach (var e in repo.GetAll()) Console.WriteLine(e);
    }

    static void ManageGrades(GradeRepository repo)
    {
        Console.WriteLine("--- Grade Management ---");
        Console.Write("StudentId: "); string sid = Console.ReadLine();
        Console.Write("CourseId: "); string cid = Console.ReadLine();
        Console.Write("Score: "); double sc = double.Parse(Console.ReadLine());
        repo.Add(new Grade { StudentId = sid, CourseId = cid, Score = sc });
        foreach (var g in repo.GetAll()) Console.WriteLine(g);
    }

    static void ManageReports(ReportService report)
    {
        Console.WriteLine("--- Reports ---");
        Console.WriteLine("1. Students by Course | 2. Grades by Course | 3. Average GPA");
        int choice = int.Parse(Console.ReadLine());
        if (choice == 1)
        {
            Console.Write("CourseId: "); string cid = Console.ReadLine();
            report.ReportStudentsByCourse(cid);
        }
        else if (choice == 2)
        {
            Console.Write("CourseId: "); string cid = Console.ReadLine();
            report.ReportGradesByCourse(cid);
        }
        else if (choice == 3)
        {
            report.ReportAverageGPA();
        }
    }
}
