namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authors = context
                .Authors
                .Select(x => new
                {
                    AuthorName = x.FirstName + " " + x.LastName,
                    Books = x.AuthorsBooks
                        .Select(b => b.Book)
                        .OrderByDescending(b => b.Price)
                        .Select(b => new
                        {
                            BookName = b.Name,
                            BookPrice = b.Price.ToString("f2")
                        })
                        .ToArray()
                })
                .ToArray()
                .OrderByDescending(x => x.Books.Length)
                .ThenBy(x => x.AuthorName)
                .ToArray();

            string json = JsonConvert.SerializeObject(authors, Formatting.Indented);

            return json;
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);


            XmlSerializer xmlSerializer =
               new XmlSerializer(typeof(ExportBookDTO[]), new XmlRootAttribute("Books"));

            var books = context
                .Books
                .ToArray()
                .Where(x => x.PublishedOn < date && x.Genre == Data.Models.Enums.Genre.Science)
                .OrderByDescending(x => x.Pages)
                .ThenByDescending(x => x.PublishedOn)
                .Take(10)
                .Select(x => new ExportBookDTO
                {
                    Pages = x.Pages,
                    Name = x.Name,
                    PublishedOn = x.PublishedOn.ToString("d", CultureInfo.InvariantCulture)
                })
                .ToArray();

            xmlSerializer.Serialize(new StringWriter(sb), books, namespaces);

            return sb.ToString().Trim();
        }
    }
}