using ProductShop.Dtos._8_exercise;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("User")]
    public class ExportUserExportDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }

        [XmlElement("lastName")]
        public string LastNaem { get; set; }

        [XmlElement("age")]
        public int? Age { get; set; }

        public SoldProductsDTO SoldProducts { get; set; }
    }
}
