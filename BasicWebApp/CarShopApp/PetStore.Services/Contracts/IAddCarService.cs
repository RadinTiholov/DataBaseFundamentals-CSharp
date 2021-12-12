using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services.Contracts
{
    public interface IAddCarService
    {
        void Add( string carMake, string carModel, string carYear, string carPictureURL);
    }
}
