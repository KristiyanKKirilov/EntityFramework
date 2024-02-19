namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var serializer = new XmlSerializer(typeof(ExportCreatorDto[]), new XmlRootAttribute("Creators"));
            StringBuilder sb = new();
            
            var creators = context.Creators
                .Where(c => c.Boardgames.Any())
                .Select(c => new ExportCreatorDto()
                {
                    CreatorName = c.FirstName + " " + c.LastName,
                    BoardgamesCount = c.Boardgames.Count(),
                    Boardgames = c.Boardgames
                    .OrderBy(b => b.Name)
                    .Select(b => new ExportCreatorBoardgameDto()
                    {
                        BoardgameName = b.Name,
                        BoardgameYearPublished = b.YearPublished,
                    }).ToArray()
                }).OrderByDescending(c => c.BoardgamesCount)
                    .ThenBy(c => c.CreatorName)
                .ToArray();

            XmlSerializerNamespaces namespaces = new();
            namespaces.Add(string.Empty, string.Empty);
            using StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, creators, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year && b.Boardgame.Rating <= rating))
                .Select(s => new ExportSellerDto()
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)
                    .OrderByDescending(bs => bs.Boardgame.Rating)
                        .ThenBy(bs => bs.Boardgame.Name)
                    .ToArray()
                    .Select(bs => new ExportBoardgameDto()
                    {
                        Name = bs.Boardgame.Name,
                        Rating = bs.Boardgame.Rating,
                        Mechanics = bs.Boardgame.Mechanics,
                        Category = bs.Boardgame.CategoryType.ToString()

                    }).ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Count())
                    .ThenBy(s => s.Name)
                    .Take(5)
                    .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}