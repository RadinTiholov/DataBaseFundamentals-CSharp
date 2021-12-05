using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class ExportPlayDTO
    {
        [XmlAttribute("Title")]
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; }

        [XmlAttribute("Duration")]
        [Required]
        public string Duration { get; set; }

        [XmlAttribute("Rating")]
        [Required]
        public string Rating { get; set; }

        [XmlAttribute("Genre")]
        [Required]
        public string Genre { get; set; }

        [XmlArray("Actors")]
        public ExportCastDTO[] Actors { get; set; }
    }
}
