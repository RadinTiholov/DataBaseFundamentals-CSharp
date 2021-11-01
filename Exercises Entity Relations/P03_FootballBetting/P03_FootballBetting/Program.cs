using P03_FootballBetting.Data;
using System;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext context = new FootballBettingContext();

            context.Database.EnsureCreated();
            Console.WriteLine("DB created");
            var color = new Data.Models.Color();
            color.Name = "Adad";

            context.Colors.Add(color);

            context.SaveChanges();
        }
    }
}
