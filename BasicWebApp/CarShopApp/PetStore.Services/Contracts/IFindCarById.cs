using CarShop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services.Contracts
{
    public interface IFindCarById
    {
        Car Find(int id);
    }
}
