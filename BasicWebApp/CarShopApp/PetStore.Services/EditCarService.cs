using CarShop.Data;
using CarShop.Models;
using PetStore.Services.Contracts;
using System.Linq;

namespace PetStore.Services
{
    public class EditCarService : IEditCarService
    {
        private CarShopContext context;
        public EditCarService()
        {
            context = new CarShopContext();
            context.Database.EnsureCreated();
        }
        public void Edit(string id, string carMake, string carModel, string carYear, string carPictureURL, string carOwner)
        {
            Car car = context.Cars.FirstOrDefault(x=> x.Id == int.Parse(id));
            car.Make = carMake;
            car.Model = carModel;
            car.Year = int.Parse(carYear);
            car.PictureURL = carPictureURL;
            car.Owner = carOwner;
            context.SaveChanges();
        }
    }
}
