using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Actor")]
    public class ExportCastDTO
    {
        [XmlAttribute("FullName")]
        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string FullName { get; set; }

        [XmlAttribute("MainCharacter")]
        [Required]
        public string MainCharacter { get; set; }
    }
}
