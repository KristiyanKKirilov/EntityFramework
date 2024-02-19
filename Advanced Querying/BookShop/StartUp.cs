namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);
            //int input = int.Parse(Console.ReadLine());
            Console.WriteLine(RemoveBooks(db));
            
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (!Enum.TryParse<AgeRestriction>(command, true, out var ageRestriction))
            {
                return $"{command} is not a valid age restriction";
            }

            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => new
                {
                    b.Title
                })
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .ToList()
                .OrderBy(b => b.BookId);

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                }).ToList()
                .OrderByDescending(b => b.Price);

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - ${b.Price:f2}"));

        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .ToList()
                .OrderBy(b => b.BookId);

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToLower()).ToArray();
            var books = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.BookCategories
                })
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title));
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                }).Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate).ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}"));
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Select(b => new
                {
                    b.FirstName,
                    b.LastName
                }).Where(b => b.FirstName
                .EndsWith(input))
                .ToList()
                .OrderBy(b => b.FirstName)
                .ThenBy(b => b.LastName);

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FirstName} {a.LastName}"));
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Select(b => new
                {
                    b.Title
                })
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => b.Title)).TrimEnd();
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Select(b => new
                {
                    b.Title,
                    b.BookId,
                    AuthorFirstName = b.Author.FirstName,
                    AuhtorLastName = b.Author.LastName
                }).Where(b => b.AuhtorLastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.AuthorFirstName} {b.AuhtorLastName})"));
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Select(b => new
                {
                    b.Title
                })
                .Where(b => b.Title.Length > lengthCheck)
                .ToList();

            return books.Count;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    FullName = string.Join(' ', a.FirstName, a.LastName),
                    TotalCopies = a.Books.Sum(b => b.Copies)
                }).OrderByDescending(a => a.TotalCopies).ToList();

            return string.Join(Environment.NewLine, authors.Select(a => $"{a.FullName} - {a.TotalCopies}"));
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var profitByCategory = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                }).OrderByDescending(cb => cb.TotalProfit)
                .ThenBy(cb => cb.CategoryName)
                .ToList();

            return string.Join(Environment.NewLine, profitByCategory.Select(cb => $"{cb.CategoryName} ${cb.TotalProfit:f2}"));
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    MostRecentBooks = c.CategoryBooks.Select(cb => cb.Book).OrderByDescending(b => b.ReleaseDate).Take(3).ToList()
                }).OrderBy(c => c.CategoryName).ToList();
            StringBuilder sb = new();
            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.MostRecentBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {

            var books = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010);
                
            foreach (var item in books)
            {
                item.Price += 5;
            }
                

            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books.Where(b => b.Copies < 4200);
            int count = books.Count();

            context.RemoveRange(books);
            context.SaveChanges();
            return count;
        }
    }
}


