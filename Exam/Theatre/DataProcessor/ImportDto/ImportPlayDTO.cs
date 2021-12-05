using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(4)]
        public string Title { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        [MaxLength(700)]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Screenwriter { get; set; }
    }
}
