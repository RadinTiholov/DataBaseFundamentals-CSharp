using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastDTO
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string FullName { get; set; }

        [Required]
        public bool IsMainCharacter { get; set; }

        [Required]
        [RegularExpression(@"\+44-\d{2}-\d{3}-\d{4}")]
        public string PhoneNumber { get; set; }

        [Required]
        public int PlayId { get; set; }
    }
}
