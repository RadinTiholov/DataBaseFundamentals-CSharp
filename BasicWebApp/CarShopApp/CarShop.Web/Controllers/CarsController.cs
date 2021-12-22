using CarShop.Models;
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
        private readonly IRemoveCarService removeCarService;
        private readonly IFindCarById findCarById;
        private readonly IEditCarService editCarService;
        private readonly IAddLikeCarService addLikeCarService;
        public CarsController(IAllCarsService allCarsService, IAddCarService addCarService, IRemoveCarService removeCarService, IFindCarById findCarById, IEditCarService editCarService, IAddLikeCarService addLikeCarService)
        {
            this.allCarsService = allCarsService;
            this.addCarService = addCarService;
            this.removeCarService = removeCarService;
            this.findCarById = findCarById;
            this.editCarService = editCarService;
            this.addLikeCarService = addLikeCarService;
        }
        public IActionResult Index()
        {
            var cars = allCarsService.All();

            return View(cars);
        }
        [HttpGet]
        public IActionResult AddCar()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult AddCar(string carMake, string carModel, string carYear, string carPictureURL)
        {
            addCarService.Add(carMake, carModel, carYear, carPictureURL);
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            removeCarService.Remove(id);
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            Car car = findCarById.Find(id);
            return View(car);
        }
        public IActionResult Like(int id)
        {
            addLikeCarService.Like(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditCar(int id)
        {
            Car car = findCarById.Find(id);
            return View(car);
        }
        [HttpPost]
        public IActionResult EditCar(string carId, string carMake, string carModel, string carYear, string carPictureURL)
        {
            editCarService.Edit(carId, carMake, carModel, carYear, carPictureURL);
            return RedirectToAction("Index");
        }
    }
}
