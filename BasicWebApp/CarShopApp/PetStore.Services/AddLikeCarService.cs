using CarShop.Data;
using PetStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetStore.Services
{
    public class AddLikeCarService: IAddLikeCarService
    {
        private CarShopContext context;
        public AddLikeCarService()
        {
            context = new CarShopContext();
            context.Database.EnsureCreated();
        }

        public void Like(int id)
        {
            var car = context.Cars.FirstOrDefault(x => x.Id == id);
            car.Likes++;
            context.SaveChanges();
        }
    }
}
