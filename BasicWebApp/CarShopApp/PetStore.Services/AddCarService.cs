using CarShop.Data;
using CarShop.Models;
using PetStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services
{
    public class AddCarService : IAddCarService
    {
        private CarShopContext context;
        public AddCarService()
        {
            context = new CarShopContext();
            context.Database.EnsureCreated();
        }
        public void Add(string carMake, string carModel, string carYear, string carPictureURL, string carOwner)
        {
            Car car = new Car
            {
                Make = carMake,
                Model = carModel,
                Year = int.Parse(carYear),
                PictureURL = carPictureURL,
                Owner = carOwner
            };
            context.Cars.Add(car);
            context.SaveChanges();
        }
    }
}
