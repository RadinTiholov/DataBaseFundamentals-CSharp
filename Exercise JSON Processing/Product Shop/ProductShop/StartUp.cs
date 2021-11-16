using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            string jsonTextProducts = File.ReadAllText("../../../Datasets/products.json");
            string jsonTextUsers = File.ReadAllText("../../../Datasets/users.json");
            string jsonTextCat = File.ReadAllText("../../../Datasets/categories.json");
            string jsonTextCatAndProd = File.ReadAllText("../../../Datasets/categories-products.json");

            Console.WriteLine(ImportUsers(context, jsonTextUsers));
            Console.WriteLine(ImportProducts(context, jsonTextProducts));
            Console.WriteLine(ImportCategories(context, jsonTextCat));
            Console.WriteLine(ImportCategoryProducts(context, jsonTextCatAndProd));

            Console.WriteLine(GetCategoriesByProductsCount(context));
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            //TODO
            var categories = context
                .Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count,
                    AvaragePrice = $"{x.CategoryProducts.Average(c => c.Product.Price):F2}",
                    TotalRavenue = $"{x.CategoryProducts.Sum(c => c.Product.Price):F2}"
                })
                .ToList();

            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = contractResolver
            };

            var categoriesJson = JsonConvert.SerializeObject(categories, settings);

            return categoriesJson;
        }
        public static string GetSoldProducts(ProductShopContext context) 
        {
            var users = context.Users
                .Include(x => x.ProductsSold)
                .ThenInclude(x => x.Buyer)
                .Where(x => x.ProductsSold.Count > 0 && x.ProductsSold.Any(b => b.Buyer != null))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    SoldProducts = x.ProductsSold.Select(product => new
                    {
                        product.Name,
                        product.Price,
                        BuyerFirstName = product.Buyer.FirstName,
                        BuyerLastName = product.Buyer.LastName
                    })
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName);

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(users,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = resolver
                });

            return json;
        }
        public static string GetUsersWithProducts(ProductShopContext context) 
        {
            var users = context
                .Users
                .AsEnumerable()
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                p.Name,
                                p.Price
                            })
                    }
                })
                .ToList();

            var output = new
            {
                UsersCount = users.Count,
                Users = users
            };

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(output,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = resolver,
                    NullValueHandling = NullValueHandling.Ignore
                });

            return json;
        }
        public static string GetProductsInRange(ProductShopContext context) 
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000).Select(x => new
                {
                    x.Name,
                    x.Price,
                    Seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .OrderBy(x => x.Price);

            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(products,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    ContractResolver = resolver
                });

            return json;
        }
        public static string ImportUsers(ProductShopContext context, string inputJson) 
        {
            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(inputJson);

            context.Users.AddRange(users);

            context.SaveChanges();

            return $"Successfully imported {users.ToList().Count}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson) 
        {
            var products = JsonConvert.DeserializeObject<IEnumerable<InputProductDTO>>(inputJson);
            InitializeMapper();

            var mappedProducts = mapper.Map<IEnumerable<Product>>(products);
            context.Products.AddRange(mappedProducts);
            context.SaveChanges();

            return $"Successfully imported {products.ToList().Count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson) 
        {
            var categories = JsonConvert.DeserializeObject<IEnumerable<InputCategoriesDTO>>(inputJson);
            InitializeMapper();

            var mappedCategories = mapper.Map<IEnumerable<Category>>(categories).Where(x => !String.IsNullOrEmpty(x.Name));
            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();

            return $"Successfully imported {mappedCategories.ToList().Count}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesAndProd = JsonConvert.DeserializeObject<IEnumerable<InputCatAndProdDTO>>(inputJson);
            InitializeMapper();

            var mappedCategoriesAndProducts = mapper.Map<IEnumerable<CategoryProduct>>(categoriesAndProd);
            context.CategoryProducts.AddRange(mappedCategoriesAndProducts);
            context.SaveChanges();

            return $"Successfully imported {mappedCategoriesAndProducts.ToList().Count}";
        }
        private static void InitializeMapper()
        {
            var mapperConfig = new MapperConfiguration(cnf => cnf.AddProfile<ProductShopProfile>());
            mapper = new Mapper(mapperConfig);
        }
    }
}