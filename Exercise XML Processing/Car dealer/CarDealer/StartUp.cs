using CarDealer.Data;
using CarDealer.DTOs;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string suppliersPath = File.ReadAllText("../../../Datasets/suppliers.xml");
            string partsPath = File.ReadAllText("../../../Datasets/parts.xml");
            string carsPath = File.ReadAllText("../../../Datasets/cars.xml");
            string customersPath = File.ReadAllText("../../../Datasets/customers.xml");
            string salesPath = File.ReadAllText("../../../Datasets/sales.xml");

            System.Console.WriteLine(ImportSuppliers(context, suppliersPath));
            System.Console.WriteLine(ImportParts(context, partsPath));
            System.Console.WriteLine(ImportCars(context, carsPath));
            System.Console.WriteLine(ImportCustomers(context, customersPath));
            System.Console.WriteLine(ImportSales(context, salesPath));

            Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context) 
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var sales = context
                .Sales
                .Select(x => new ExportSaleDTO()
                {
                    Car = new ExportCarsWithDistanceDTO()
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    CustomerName = x.Customer.Name,
                    Discount = x.Discount,
                    Price = x.Car.PartCars.Sum(c => c.Part.Price),
                    PriceWithDiscount = x.Car.PartCars.Sum(c => c.Part.Price) -
                                        x.Car.PartCars.Sum(c => c.Part.Price) * x.Discount / 100
                })
                .ToArray();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSaleDTO[]), new XmlRootAttribute("sales"));

            xmlSerializer.Serialize(new StringWriter(sb), sales, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context) 
        {
            var customers = context
                .Customers
                .Select(x => new ExportSaleCustomerDTO()
                {
                    Name = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(s => s.Car.PartCars.Sum(p => p.Part.Price))
                })
                .Where(c => c.BoughtCars >= 1)
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportSaleCustomerDTO[]), new XmlRootAttribute("customers"));

            xmlSerializer.Serialize(new StringWriter(sb), customers, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context) 
        {
            throw new NotImplementedException();
        }
        public static string GetLocalSuppliers(CarDealerContext context) 
        {
            //TODO 
            StringBuilder sb = new StringBuilder();

            var suppliers = context
                .Suppliers
                .Where(s => !s.IsImporter)
                .Select(x => new LocalSuppliersDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(LocalSuppliersDTO[]), new XmlRootAttribute("suppliers"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            xmlSerializer.Serialize(new StringWriter(sb), suppliers, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context) 
        {
            var cars = context
                .Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new ExportBMWCarDTO 
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportBMWCarDTO[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", ""); ;

            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCarsWithDistance(CarDealerContext context) 
        {
            var cars = context.Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(x => new ExportCarsWithDistanceDTO
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportCarsWithDistanceDTO[]), new XmlRootAttribute("cars"));

            StringBuilder sb = new StringBuilder();

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", ""); ;

            xmlSerializer.Serialize(new StringWriter(sb), cars, namespaces);

            return sb.ToString().Trim();
        }
        public static string ImportSales(CarDealerContext context, string inputXml) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDTO[]), new XmlRootAttribute("Sales"));

            ImportSaleDTO[] salesDtos = (ImportSaleDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Sale> sales = new List<Sale>();
            foreach (var saleDto in salesDtos)
            {
                if (context.Cars.Any(c => c.Id == saleDto.CarId))
                {
                    Sale sale = new Sale()
                    {
                        CarId = saleDto.CarId,
                        CustomerId = saleDto.CustomerId,
                        Discount = saleDto.Discount
                    };
                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomerDTO[]), new XmlRootAttribute("Customers"));
            var customerDTOs = (ImportCustomerDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Customer> customers = new List<Customer>();
            foreach (var DTO in customerDTOs)
            {
                DateTime date;
                bool isValidDate = DateTime.TryParseExact(DTO.BirthDate, "yyyy-MM-dd'T'HH:mm:ss",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

                if (isValidDate) 
                {
                    Customer customer = new Customer
                    {
                        Name = DTO.Name,
                        BirthDate = date,
                        IsYoungDriver = bool.Parse(DTO.IsYoungDriver)
                    };

                    customers.Add(customer);
                } 
            }
            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarDTO[]), new XmlRootAttribute("Cars"));
            var carDTOs = (ImportCarDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Car> cars = new List<Car>();

            List<PartCar> partCars = new List<PartCar>();
            foreach (var DTO in carDTOs)
            {
                var parts = DTO
                    .Parts
                    .Where(pc => context.Parts.Any(p => p.Id == pc.Id))
                    .Select(p => p.Id)
                    .Distinct();

                Car car = new Car 
                {
                    Make = DTO.Make,
                    Model = DTO.Model,
                    TravelledDistance = DTO.TraveledDistance,
                    PartCars = parts as ICollection<PartCar>
                };

                foreach (var part in parts)
                {
                    PartCar partCar = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);
            }
            context.AddRange(cars);
            context.AddRange(partCars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDTO[]), new XmlRootAttribute("Parts"));
            var partDTOs = (ImportPartDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Part> parts = new List<Part>();
            foreach (var DTO in partDTOs)
            {
                Supplier supplier = context.Suppliers.Find(DTO.SupplierId);
                if (supplier == null)
                {
                    continue;
                }

                Part part = new Part
                {
                    Name = DTO.Name,
                    Price = decimal.Parse(DTO.Price),
                    Quantity = DTO.Quantity,
                    SupplierId = DTO.SupplierId,
                    Supplier = supplier
                };

                parts.Add(part);
            }
            context.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDTO[]), new XmlRootAttribute("Suppliers"));

            var suppliersDtos = (ImportSupplierDTO[])xmlSerializer.Deserialize(new StringReader(inputXml));

            List<Supplier> suppliers = new List<Supplier>();
            foreach (var DTO in suppliersDtos)
            {
                var supplier = new Supplier
                {
                    Name = DTO.Name,
                    IsImporter = bool.Parse(DTO.IsImporter)
                };
                suppliers.Add(supplier);
            }

            context.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }
    }
}