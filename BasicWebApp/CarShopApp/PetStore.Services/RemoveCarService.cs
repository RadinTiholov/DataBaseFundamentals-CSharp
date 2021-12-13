using CarShop.Data;
using CarShop.Models;
using PetStore.Services.Contracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services
{
    public class RemoveCarService : IRemoveCarService
    {
        private CarShopContext context;
        public RemoveCarService()
        {
            context = new CarShopContext();
            context.Database.EnsureCreated();
        }
        public void Remove(int id)
        {
            Car car = context.Cars.FirstOrDefault(x => x.Id == id);
            context.Cars.Remove(car);
            context.SaveChanges();
        }
    }
}
