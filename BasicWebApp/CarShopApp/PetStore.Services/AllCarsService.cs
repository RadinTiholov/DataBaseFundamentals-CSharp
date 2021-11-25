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
        public List<Car> All()
        {
            return context.Cars.ToList();
        }
    }
}
