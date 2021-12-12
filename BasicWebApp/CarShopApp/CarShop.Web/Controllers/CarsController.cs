using Microsoft.AspNetCore.Mvc;
using PetStore.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Web.Controllers
{
    public class CarsController : Controller
    {
        private readonly IAllCarsService allCarsService;
        private readonly IAddCarService addCarService;
        public CarsController(IAllCarsService allCarsService, IAddCarService addCarService)
        {
            this.allCarsService = allCarsService;
            this.addCarService = addCarService;
        }
        public IActionResult Index()
        {
            var cars = allCarsService.All();

            return View(cars);
        }
        public IActionResult AddCarRedirect()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult AddCar(string carMake, string carModel, string carYear, string carPictureURL)
        {
            addCarService.Add(carMake, carModel, carYear, carPictureURL);
            return RedirectToAction("Index");
        }
    }
}
