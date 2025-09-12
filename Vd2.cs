using System;
using System.Collections.Generic;
using System.Linq;

public class Student
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public double GPA { get; set; }

    public override string ToString()
    {
        return $"ID:{Id} | Name:{Name} | Age:{Age} | GPA:{GPA}";
    }
}

public class StudentService
{
    private List<Student> students = new List<Student>();

    public void Add(Student student) => students.Add(student);

    public void Remove(string id) =>
        students.RemoveAll(s => s.Id == id);

    public void Update(string id, string name, int age, double gpa)
    {
        var student = students.Find(s => s.Id == id);
        if (student != null)
        {
            student.Name = name;
            student.Age = age;
            student.GPA = gpa;
        }
        else
        {
            Console.WriteLine("Không tìm thấy sinh viên để cập nhật.");
        }
    }

    public void DisplayAll()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("Danh sách rỗng.");
            return;
        }
        foreach (var s in students)
            Console.WriteLine(s);
    }

    public void SearchByName(string name)
    {
        var result = students.Where(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList();
        if (result.Count == 0)
            Console.WriteLine("Không tìm thấy sinh viên.");
        else
            result.ForEach(s => Console.WriteLine("Tìm thấy: " + s));
    }

    public void DisplayExcellent()
    {
        var result = students.Where(s => s.GPA > 8.0).ToList();
        if (result.Count == 0)
            Console.WriteLine("Không có sinh viên nào GPA > 8.");
        else
            result.ForEach(s => Console.WriteLine("Sinh viên giỏi: " + s));
    }

    public void SortByName()
    {
        students = students.OrderBy(s => s.Name).ToList();
        Console.WriteLine("Đã sắp xếp theo tên.");
    }

    public void SortByGPA()
    {
        students = students.OrderByDescending(s => s.GPA).ToList();
        Console.WriteLine("Đã sắp xếp theo GPA.");
    }
}

class Program
{
    static void Main(string[] args)
    {
        var service = new StudentService();
        int menu = 0;

        while (menu != 99)
        {
            Console.WriteLine("============= MENU =============");
            Console.WriteLine("1. Thêm SV");
            Console.WriteLine("2. Xóa SV");
            Console.WriteLine("3. Cập nhật SV");
            Console.WriteLine("4. Hiển thị tất cả SV");
            Console.WriteLine("5. Tìm SV theo tên");
            Console.WriteLine("6. Sinh viên GPA > 8");
            Console.WriteLine("7. Sắp xếp theo tên");
            Console.WriteLine("8. Sắp xếp theo GPA");
            Console.WriteLine("99. Thoát");
            Console.Write("Nhập lựa chọn: ");

            if (!int.TryParse(Console.ReadLine(), out menu))
            {
                Console.WriteLine("Vui lòng nhập số hợp lệ!");
                continue;
            }

            switch (menu)
            {
                case 1:
                    Console.Write("ID: ");
                    string id = Console.ReadLine();
                    Console.Write("Tên: ");
                    string name = Console.ReadLine();
                    Console.Write("Tuổi: ");
                    int age = int.Parse(Console.ReadLine());
                    Console.Write("GPA: ");
                    double gpa = double.Parse(Console.ReadLine());
                    service.Add(new Student { Id = id, Name = name, Age = age, GPA = gpa });
                    break;

                case 2:
                    Console.Write("Nhập ID cần xóa: ");
                    service.Remove(Console.ReadLine());
                    break;

                case 3:
                    Console.Write("ID: ");
                    string uid = Console.ReadLine();
                    Console.Write("Tên mới: ");
                    string uname = Console.ReadLine();
                    Console.Write("Tuổi mới: ");
                    int uage = int.Parse(Console.ReadLine());
                    Console.Write("GPA mới: ");
                    double ugpa = double.Parse(Console.ReadLine());
                    service.Update(uid, uname, uage, ugpa);
                    break;

                case 4:
                    service.DisplayAll();
                    break;

                case 5:
                    Console.Write("Tên: ");
                    service.SearchByName(Console.ReadLine());
                    break;

                case 6:
                    service.DisplayExcellent();
                    break;

                case 7:
                    service.SortByName();
                    break;

                case 8:
                    service.SortByGPA();
                    break;

                case 99:
                    Console.WriteLine("Thoát chương trình.");
                    break;

                default:
                    Console.WriteLine("Lựa chọn không hợp lệ.");
                    break;
            }
            Console.WriteLine();
        }
    }
}
