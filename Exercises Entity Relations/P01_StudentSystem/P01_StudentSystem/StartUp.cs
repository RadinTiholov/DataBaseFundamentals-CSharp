using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            StudentSystemContext context = new StudentSystemContext();
            Course course = new Course();
            course.Name = "ASdasd";
            course.StartDate = new DateTime();
            course.Price = 12.3M;

            Resource res = new Resource();
            res.Name = "OOP";
            res.Url = "asdasda";
            res.ResourceType = Data.Enumerations.ResourceType.Video;
            res.CourseId = 1;

            context.Courses.Add(course);
            context.Resources.Add(res);

            context.SaveChanges();
        }
    }
}
