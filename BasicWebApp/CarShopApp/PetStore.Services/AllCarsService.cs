using CarShop.Data;
using CarShop.Models;
using PetStore.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace PetStore.Services
{
    public class AllCarsService : IAllCarsService
    {
        private CarShopContext context;
        public AllCarsService()
        {
            context = new CarShopContext();
            context.Database.EnsureCreated();
        }
        public List<Car> All(int id)
        {
            if (id == 1)
            {
                return context.Cars.OrderBy(x => x.Make).ToList();
            }
            else if (id == 2)
            {
                return context.Cars.OrderByDescending(x => x.Make).ToList();
            }
            else
            {
                return context.Cars.ToList();
            }
        }

        public List<Car> Search(string searchQuery)
        {
            return context.Cars.Where(x => x.Model.Contains(searchQuery) || x.Make.Contains(searchQuery)).ToList();
        }
    }
}
