namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            string input = Console.ReadLine();
            Console.WriteLine(GetMostRecentBooks(db));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command) 
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .ToArray()
                .OrderBy(x => x.Title)
                .ToArray();

            StringBuilder text = new StringBuilder();
            foreach (var book in books)
            {
                text.AppendLine(book.Title);
            }
            return text.ToString().Trim();
        }
        public static string GetGoldenBooks(BookShopContext context) 
        {
            var goldenBooks = context.Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .ToArray()
                .OrderBy(x => x.BookId);

            StringBuilder text = new StringBuilder();
            foreach (var book in goldenBooks)
            {
                text.AppendLine(book.Title);
            }
            return text.ToString().Trim();

        }
        public static string GetBooksByPrice(BookShopContext context) 
        {
            var books = context.Books.Where(x => x.Price > 40).ToArray().OrderByDescending(x => x.Price);

            StringBuilder text = new StringBuilder();
            foreach (var book in books)
            {
                text.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return text.ToString().Trim();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year) 
        {
            var books = context.Books.Where(x => x.ReleaseDate.Value.Year != year).ToArray().OrderBy(x => x.BookId);

            StringBuilder text = new StringBuilder();
            foreach (var book in books)
            {
                text.AppendLine($"{book.Title}");
            }
            return text.ToString().Trim();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            List<string> categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var books = context.BooksCategories
                .Select(b => new
                {
                    b.Category,
                    b.Book.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in books)
            {
                if (categories.Contains(book.Category.Name.ToLower()))
                {
                    sb.AppendLine($"{book.Title}");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date) 
        {
            var books = context
                .Books
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price,
                    x.ReleaseDate
                })
                .Where(x => x.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .ToArray()
                .OrderByDescending(x => x.ReleaseDate);

            StringBuilder text = new StringBuilder();
            foreach (var book in books)
            {
                text.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }
            return text.ToString().Trim();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input) 
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    x.FirstName,
                    FullName = $"{x.FirstName} {x.LastName}"
                })
                .ToArray()
                .OrderBy(x => x.FullName);

            StringBuilder text = new StringBuilder();
            foreach (var author in authors)
            {
                text.AppendLine($"{author.FullName}");
            }
            return text.ToString().Trim();
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input) 
        {
            input = input.ToLower();

            return string.Join(Environment.NewLine, context.Books
                .Where(b => b.Title.ToLower().Contains(input))
                .Select(b => b.Title)
                .OrderBy(t => t));
        }
        public static string GetBooksByAuthor(BookShopContext context, string input) 
        {
            input = input.ToLower();

            return string.Join(Environment.NewLine, context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(b => b.BookId)
                .Select(b => b.Author.FirstName == null
                    ? $"{b.Title} ({b.Author.LastName})"
                    : $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})"));
        }
        public static int CountBooks(BookShopContext context, int lengthCheck) 
        {
            var books = context
                .Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToArray();

            return books.Count();
        }
        public static string CountCopiesByAuthor(BookShopContext context)
            =>
        string.Join(Environment.NewLine, context.Authors
                .Select(a => new
                {
                    Name = a.FirstName == null 
                        ? a.LastName 
                        : $"{a.FirstName} {a.LastName}",
                    Copies = a.Books
                        .Select(b => b.Copies)
                        .Sum()
                })
                .OrderByDescending(a => a.Copies)
                .Select(a => $"{a.Name} - {a.Copies}"));

        public static string GetTotalProfitByCategory(BookShopContext context)
         => string.Join(Environment.NewLine, context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    TotalProffit = c.CategoryBooks
                        .Select(cb => cb.Book.Price * cb.Book.Copies)
                        .Sum()
                })
                .OrderByDescending(c => c.TotalProffit)
                .ThenBy(c => c.Name)
                .Select(c => $"{c.Name} ${c.TotalProffit:F2}"));

        public static void IncreasePrices(BookShopContext context) 
        {
            var books = context
                .Books
                .Where(x => x.ReleaseDate != null && x.ReleaseDate.Value.Year < 2010)
                .ToArray();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context) 
        {
            var books = context
                .Books
                .Where(x => x.Copies < 4200)
                .ToArray();

            context.Books.RemoveRange(books);

            context.SaveChanges();

            return books.Length;
        }
        public static string GetMostRecentBooks(BookShopContext context) 
        {
            var categories = context
                .Categories
                .Select(x => new
                {
                    x.Name,
                    Books = x.CategoryBooks.Select(x => x.Book).OrderByDescending(x => x.ReleaseDate).Take(3)
                })
                .ToArray()
                .OrderBy(x => x.Name);

            StringBuilder text = new StringBuilder();
            foreach (var category in categories)
            {
                text.AppendLine($"--{category.Name}");
                foreach (var books in category.Books)
                {
                    text.AppendLine($"{books.Title} ({books.ReleaseDate.Value.Year})");
                }
            }
            return text.ToString().Trim();
        }
    }
}
