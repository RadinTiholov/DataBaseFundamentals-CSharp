using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<InputSupplierDTO, Supplier>();
            CreateMap<InputPartsDTO, Part>();
            CreateMap<InputCustumerDTO, Customer>();
            CreateMap<InputCarDTO, Car>();
            CreateMap<InputSalesDTO, Sale>();
        }
    }
}
