using CarShop.Data;
using CarShop.Models;
using PetStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetStore.Services
{
    public class FindCarById : IFindCarById
    {
        private CarShopContext context;
        public FindCarById()
        {
            context = new CarShopContext();
            context.Database.EnsureCreated();
        }
        public Car Find(int id)
        {
            var car = context.Cars.FirstOrDefault(x => x.Id == id);
            return car;
        }
    }
}
