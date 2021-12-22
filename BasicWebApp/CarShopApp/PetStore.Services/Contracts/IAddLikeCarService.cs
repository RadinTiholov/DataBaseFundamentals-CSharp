using System;
using System.Collections.Generic;
using System.Text;

namespace PetStore.Services.Contracts
{
    public interface IAddLikeCarService
    {
        void Like(int id);
    }
}
