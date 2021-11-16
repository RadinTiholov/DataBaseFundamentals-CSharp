using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealer.DTO
{
    class InputCarDTO
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public long TravelledDistance { get; set; }
        public ICollection<int> Parts { get; set; } = new List<int>();
    }
}
