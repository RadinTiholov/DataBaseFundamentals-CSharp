﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos._8_exercise
{
    [XmlType("SoldProducts")]
    public class SoldProductsDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public ExportProductSoldDTO[] Products { get; set; }
    }
}
