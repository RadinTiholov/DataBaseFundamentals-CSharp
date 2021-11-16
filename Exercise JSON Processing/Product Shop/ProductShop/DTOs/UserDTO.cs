using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.DTOs
{
    [Serializable]
    class UserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }
    }
}
