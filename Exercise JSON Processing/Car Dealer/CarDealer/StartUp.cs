using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string jsonTextSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");
            string jsonTextPart = File.ReadAllText("../../../Datasets/parts.json");
            string jsonTextCustomers = File.ReadAllText("../../../Datasets/customers.json");
            string jsonTextCar = File.ReadAllText("../../../Datasets/cars.json");
            string jsonTextSales = File.ReadAllText("../../../Datasets/sales.json");

            Console.WriteLine(ImportSuppliers(context, jsonTextSuppliers));
            Console.WriteLine(ImportParts(context, jsonTextPart));
            Console.WriteLine(ImportCustomers(context, jsonTextCustomers));
            Console.WriteLine(ImportCars(context, jsonTextCar));
            Console.WriteLine(ImportSales(context, jsonTextSales));
            Console.WriteLine(GetTotalSalesByCustomer(context));
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context) 
        {
            var sales = context.Sales.Select(s => new
            {
                car = new
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TravelledDistance = s.Car.TravelledDistance
                },
                customerName = s.Customer.Name,
                Discount = s.Discount.ToString("f2"),
                price = s.Car.PartCars.Sum(p => p.Part.Price).ToString("f2"),
                priceWithDiscount = ((s.Car.PartCars.Sum(pc => pc.Part.Price)) * (1 - s.Discount * 0.01m))
                .ToString("f2")
            })
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(sales, Formatting.Indented);

            return json;
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context) 
        {
            var totalSales = context.Customers.Where(c => c.Sales.Count() > 0)
                                    .Select(c => new
                                    {
                                        fullName = c.Name,
                                        boughtCars = c.Sales.Count(),
                                        spentMoney = c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))
                                    })
                                    .OrderByDescending(x => x.spentMoney)
                                    .ThenByDescending(x => x.boughtCars)
                                    .ToList();


            var json = JsonConvert.SerializeObject(totalSales, Formatting.Indented);

            return json;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context) 
        {
            var cars = context.Cars.Select(c => new
            {
                car = new
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                },
                parts = c.PartCars.Select(pc => new
                {
                    Name = pc.Part.Name,
                    Price = pc.Part.Price.ToString("f2")
                }).ToList()

            }).ToList();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context) 
        {
            var cars = context.Cars
                .Where(x => x.Make == "Toyota")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new 
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);

            return json;
        }
        public static string GetLocalSuppliers(CarDealerContext context) 
        {
            var suppliers = context.Suppliers.Where(s => s.IsImporter == false)
                                             .Select(s => new
                                             {
                                                 Id = s.Id,
                                                 Name = s.Name,
                                                 PartsCount = s.Parts.Count()
                                             }).ToList();

            var json = JsonConvert.SerializeObject(suppliers, Formatting.Indented);

            return json;
        }
        public static string GetOrderedCustomers(CarDealerContext context) 
        {
            var customers = context.Customers.OrderBy(c => c.BirthDate).ThenBy(c => c.IsYoungDriver)
                                   .Select(c => new
                                   {
                                       Name = c.Name,
                                       BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                                       IsYoungDriver = c.IsYoungDriver
                                   }
                                   ).ToList();

            var json = JsonConvert.SerializeObject(customers, Formatting.Indented);

            return json;
        }
        public static string ImportSales(CarDealerContext context, string inputJson) 
        {
            InitializeMapper();
            var sales = JsonConvert.DeserializeObject<IEnumerable<InputSalesDTO>>(inputJson);
            var mappedSales = mapper.Map<IEnumerable<Sale>>(sales);

            context.Sales.AddRange(mappedSales);
            context.SaveChanges();

            return $"Successfully imported {mappedSales.ToList().Count}.";
        }
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var cars = JsonConvert.DeserializeObject<List<InputCarDTO>>(inputJson);
            var mappedCars = mapper.Map<IEnumerable<Car>>(cars);

            context.Cars.AddRange(mappedCars);

            context.SaveChanges();
            int affectedRows = context.Cars.Count();

            return $"Successfully imported {affectedRows}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson) 
        {
            InitializeMapper();
            var custumers = JsonConvert.DeserializeObject<IEnumerable<InputCustumerDTO>>(inputJson);

            var mappedCustomers = mapper.Map<IEnumerable<Customer>>(custumers);

            context.Customers.AddRange(mappedCustomers);
            context.SaveChanges();

            return $"Successfully imported {mappedCustomers.ToList().Count}.";
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson) 
        {
            var suppliers = JsonConvert.DeserializeObject<IEnumerable<InputSupplierDTO>>(inputJson);
            InitializeMapper();

            var mappedSuppliers = mapper.Map<IEnumerable<Supplier>>(suppliers);

            context.Suppliers.AddRange(mappedSuppliers);
            context.SaveChanges();

            return $"Successfully imported {mappedSuppliers.ToList().Count}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson) 
        {
            var parts = JsonConvert.DeserializeObject<IEnumerable<InputPartsDTO>>(inputJson)
                .Where(x => context.Suppliers.Any(s => s.Id == x.SupplierId));
            InitializeMapper();

            var mappedParts = mapper.Map<IEnumerable<Part>>(parts);

            context.Parts.AddRange(mappedParts);
            context.SaveChanges();

            return $"Successfully imported {mappedParts.ToList().Count}.";
        }
        private static void InitializeMapper()
        {
            var mapperConfig = new MapperConfiguration(cnf => cnf.AddProfile<CarDealerProfile>());
            mapper = new Mapper(mapperConfig);
        }
    }
}