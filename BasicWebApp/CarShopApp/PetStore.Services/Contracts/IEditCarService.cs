using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services.Contracts
{
    public interface IEditCarService
    {
        void Edit(string id, string carMake, string carModel, string carYear, string carPictureURL);
    }
}
