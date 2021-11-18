using System.Xml.Serialization;

namespace CarDealer.DTOs
{
    [XmlType("Sale")]
    public class ImportSaleDTO
    {
        [XmlElement("carId")]
        public int CarId { get; set; }

        [XmlElement("customerId")]
        public int CustomerId { get; set; }
        
        [XmlElement("discount")]
        public int Discount { get; set; }
    }
}
