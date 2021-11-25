using CarShop.Data;
using CarShop.Models;
using System;

namespace CarShop
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new CarShopContext();
            context.Database.EnsureCreated();
            var car = new Car()
            {
                Make = "Opel",
                Model = "Zafira",
                Year = 2000,
                PictureURL = "http://blog.autocredit.bg/wp-content/uploads/2015/09/zafira-1.jpg"
            };
            context.Cars.Add(car);
            context.SaveChanges();
            Console.WriteLine("kk");
        }
    }
}
