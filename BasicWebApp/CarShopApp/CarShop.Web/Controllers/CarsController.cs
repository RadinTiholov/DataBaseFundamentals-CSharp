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
        public CarsController(IAllCarsService allCarsService)
        {
            this.allCarsService = allCarsService;
        }
        public IActionResult Index()
        {
            var cars = allCarsService.All();

            return View(cars);
        }
    }
}
