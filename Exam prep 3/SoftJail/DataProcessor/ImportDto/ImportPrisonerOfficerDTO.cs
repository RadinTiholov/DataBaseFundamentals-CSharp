using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Prisoner")]
    public class ImportPrisonerOfficerDTO
    {
        [Required]
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
