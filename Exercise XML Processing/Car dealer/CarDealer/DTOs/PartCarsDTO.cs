using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("partId")]
    public class PartCarsDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
