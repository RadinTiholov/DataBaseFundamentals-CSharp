namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            StringBuilder text = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Books");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(AuthorBookDTO[]), root);

            StringReader reader = new StringReader(xmlString);

            HashSet<Book> books = new HashSet<Book>();
            using (reader)
            {
                AuthorBookDTO[] bookDTOs = (AuthorBookDTO[])xmlSerializer.Deserialize(reader);
                foreach (var bookDTO in bookDTOs)
                {
                    if (!IsValid(bookDTO))
                    {
                        text.AppendLine(ErrorMessage);
                        continue;
                    }
                    var date = DateTime.ParseExact(bookDTO.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                    Book book = new Book 
                    {
                        Name = bookDTO.Name,
                        Genre = (Genre)bookDTO.Genre,
                        Price = bookDTO.Price,
                        Pages = bookDTO.Pages,
                        PublishedOn = date
                    };
                    books.Add(book);
                    text.AppendLine(String.Format(SuccessfullyImportedBook, book.Name, book.Price));
                }
            }
            context.Books.AddRange(books);
            context.SaveChanges();

            return text.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            StringBuilder text = new StringBuilder();

            var authorDTOs = JsonConvert.DeserializeObject<IEnumerable<ImportAuthorDTO>>(jsonString);
            HashSet<Author> authors = new HashSet<Author>();

            foreach (var authorDTO in authorDTOs)
            {
                if (!IsValid(authorDTO))
                {
                    text.AppendLine(ErrorMessage);
                    continue;
                }
                if (authors.FirstOrDefault(x => x.Email == authorDTO.Email) != null)
                {
                    text.AppendLine(ErrorMessage);
                    continue;
                }
                Author author = new Author
                {
                    FirstName = authorDTO.FirstName,
                    LastName = authorDTO.LastName,
                    Phone = authorDTO.Phone,
                    Email = authorDTO.Email
                };
                foreach (var bookDTO in authorDTO.Books)
                {
                    //AuthorBook authorBook = context.AuthorsBooks.FirstOrDefault(x => x.BookId == bookDTO.Id);
                    if (!context.Books.Any(x => x.Id == bookDTO.Id) || bookDTO.Id == null)
                    {
                        continue;
                    }
                    AuthorBook authorBook = new AuthorBook 
                    {
                        AuthorId = author.Id,
                        BookId = (int)bookDTO.Id
                    };
                    author.AuthorsBooks.Add(authorBook);
                }
                if (author.AuthorsBooks.Count == 0)
                {
                    text.AppendLine(ErrorMessage);
                    continue;
                }

                authors.Add(author);
                text.AppendLine(String.Format(SuccessfullyImportedAuthor, author.FirstName + " " + author.LastName, author.AuthorsBooks.Count));
            }

            context.Authors.AddRange(authors);
            context.SaveChanges();

            return text.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}