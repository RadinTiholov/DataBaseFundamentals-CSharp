using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services.Contracts
{
    public interface IAllCarsService
    {
        List<Car> All();
    }
}
