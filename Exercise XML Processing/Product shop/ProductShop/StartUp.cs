using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ProductShop.Dtos._8_exercise;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //ProductShopContext context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //string pathUsers = File.ReadAllText("../../../Datasets/users.xml");
            //string pathProducts = File.ReadAllText("../../../Datasets/products.xml");
            //string pathCategories = File.ReadAllText("../../../Datasets/categories.xml");
            //string pathCatProd = File.ReadAllText("../../../Datasets/categories-products.xml");
            //System.Console.WriteLine(ImportUsers(context, pathUsers));
            //System.Console.WriteLine(ImportProducts(context, pathProducts));
            //System.Console.WriteLine(ImportCategories(context, pathCategories));
            //System.Console.WriteLine(ImportCategoryProducts(context, pathCatProd));

            //System.Console.WriteLine(GetUsersWithProducts(context));

        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var users = new UserRootDTO()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any(p => p.Buyer != null)),
                Users = context.Users
                    .ToArray()
                    .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Take(10)
                    .Select(u => new UserExportDTO()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new SoldProductsDTO()
                        {
                            Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                            Products = u.ProductsSold
                                .ToArray()
                                .Where(ps => ps.Buyer != null)
                                .Select(ps => new ExportProductSoldDTO()
                                {
                                    Name = ps.Name,
                                    Price = ps.Price
                                })
                                .OrderByDescending(p => p.Price)
                                .ToArray()
                        }
                    })

                    .ToArray()
            };

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserRootDTO), new XmlRootAttribute("Users"));

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context) 
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);


            XmlSerializer xmlSerializer =
               new XmlSerializer(typeof(ExportCategoryByProductsDTO[]), new XmlRootAttribute("Categories"));

            var categories = context
                .Categories
                .Select(x => new ExportCategoryByProductsDTO
                {
                    Name = x.Name,
                    AveragePrice = x.CategoryProducts.Average(s => s.Product.Price),
                    Count = x.CategoryProducts.Count,
                    TotalRevenue = x.CategoryProducts.Sum(s => s.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToArray();


            xmlSerializer.Serialize(new StringWriter(sb), categories, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(x => x.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(x => new ExportSoldProductsDTO()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Where(p => p.Buyer != null)
                        .Select(p => new ExportProductDTO()
                        {
                            Name = p.Name,
                            Price = p.Price
                        }).ToArray()
                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportSoldProductsDTO[]), new XmlRootAttribute("Users"));

            xmlSerializer.Serialize(new StringWriter(sb), users, namespaces);

            return sb.ToString().Trim();
        }
        public static string GetProductsInRange(ProductShopContext context) 
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ExportProductInRangeDTO()
                {
                    Name = p.Name,
                    BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName,
                    Price = p.Price

                })
                .ToArray();

            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(ExportProductInRangeDTO[]), new XmlRootAttribute("Products"));


            xmlSerializer.Serialize(new StringWriter(sb), products, namespaces);

            return sb.ToString().Trim();

        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml) 
        {
            XmlRootAttribute root = new XmlRootAttribute("CategoryProducts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryProductDTO[]), root);

            StringReader reader = new StringReader(inputXml);
            HashSet<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

            using (reader)
            {
                ImportCategoryProductDTO[] importDTOs = (ImportCategoryProductDTO[])xmlSerializer.Deserialize(reader);

                foreach (var DTO in importDTOs)
                {
                    Product product = context.Products.Find(DTO.ProductId);
                    Category category = context.Categories.Find(DTO.CategoryId);
                    if (product == null || category == null)
                    {
                        continue;
                    }

                    CategoryProduct cp = new CategoryProduct 
                    {
                        Category = category,
                        CategoryId = DTO.CategoryId,
                        Product = product,
                        ProductId = DTO.ProductId
                    };

                    categoryProducts.Add(cp);
                }
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml) 
        {
            XmlRootAttribute root = new XmlRootAttribute("Categories");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCategoryDTO[]), root);

            StringReader reader = new StringReader(inputXml);

            HashSet<Category> categories = new HashSet<Category>();
            using (reader)
            {
                ImportCategoryDTO[] categoryDTOs = (ImportCategoryDTO[])xmlSerializer.Deserialize(reader);

                foreach (var categoryDTO in categoryDTOs)
                {
                    if (categoryDTO.Name == null)
                    {
                        continue;
                    }
                    Category cat = new Category 
                    {
                        Name = categoryDTO.Name,
                    };
                    categories.Add(cat);
                }
            }
            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Products");
            XmlSerializer xmlSerialzer = new XmlSerializer(typeof(ImportProductDTO[]), root);

            StringReader reader = new StringReader(inputXml);

            HashSet<Product> products = new HashSet<Product>();
            using (reader)
            {
                ImportProductDTO[] productDTOs = (ImportProductDTO[])xmlSerialzer.Deserialize(reader);

                foreach (var productDTO in productDTOs)
                {
                    User seller = context.Users.Find(productDTO.SellerId);
                    User buyer = context.Users.Find(productDTO.BuyerId);
                    Product product = new Product()
                    {
                        Name = productDTO.Name,
                        Price = decimal.Parse(productDTO.Price),
                        SellerId = productDTO.SellerId,
                        Seller = seller,
                        BuyerId = productDTO.BuyerId,
                        Buyer = buyer
                    };
                    products.Add(product);
                }
            }
            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
        public static string ImportUsers(ProductShopContext context, string inputXml) 
        {
            XmlRootAttribute root = new XmlRootAttribute("Users");
            XmlSerializer xmlSerialzer = new XmlSerializer(typeof(ImportUserDTO[]), root);

            StringReader reader = new StringReader(inputXml);

            HashSet<User> users = new HashSet<User>();
            using (reader)
            {
                ImportUserDTO[] userDTOs = (ImportUserDTO[])xmlSerialzer.Deserialize(reader);

                foreach (var userDTO in userDTOs)
                {
                    User user = new User()
                    {
                        FirstName = userDTO.FirstName,
                        LastName = userDTO.LastName,
                        Age = userDTO.Age
                    };
                    users.Add(user);
                }
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}.";

        }
    }
}