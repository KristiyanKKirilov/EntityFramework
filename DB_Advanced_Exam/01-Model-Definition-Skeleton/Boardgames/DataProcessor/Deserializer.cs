namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Text.Json;
    using System.Xml.Serialization;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportCreatorDto>), new XmlRootAttribute("Creators"));
            List<ImportCreatorDto> creatorDtos = (List<ImportCreatorDto>)serializer.Deserialize(new StringReader(xmlString));
            List<Creator> creators = new();
            StringBuilder sb = new();

            foreach (var creatorDto in creatorDtos)
            {
                if (!IsValid(creatorDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Creator creatorToAdd = new () 
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName                    
                };

                foreach (var boardgame in creatorDto.Boardgames)
                {
                    if (!IsValid(boardgame))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (string.IsNullOrEmpty(boardgame.Name))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Boardgame boardgameToAdd = new()
                    {
                        Name = boardgame.Name,
                        Rating = boardgame.Rating,
                        YearPublished = boardgame.YearPublished,
                        CategoryType = (CategoryType)boardgame.CategoryType,
                        Mechanics = boardgame.Mechanics
                    };

                    creatorToAdd.Boardgames.Add(boardgameToAdd);    
                }
                creators.Add(creatorToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedCreator, creatorToAdd.FirstName, 
                    creatorToAdd.LastName, creatorToAdd.Boardgames.Count));
            }
            context.Creators.AddRange(creators);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            var sellerDtos = JsonSerializer.Deserialize<List<ImportSellerDto>>(jsonString);
            var sellers = new List<Seller>();
            StringBuilder sb = new();
            var boardgameIds = context.Boardgames.Select(b => b.Id).ToList();
            foreach (var sellerDto in sellerDtos)
            {
                if (!IsValid(sellerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var sellerToAdd = new Seller()
                {
                    Name = sellerDto.Name,
                    Address = sellerDto.Address,
                    Country = sellerDto.Country,
                    Website = sellerDto.Website
                };

                foreach (var boardgameId in sellerDto.Boardgames.Distinct())
                {
                    if(sellerToAdd.BoardgamesSellers.Any(b => b.BoardgameId == boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    if (!boardgameIds.Contains(boardgameId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    sellerToAdd.BoardgamesSellers.Add(new BoardgameSeller
                    {
                        BoardgameId = boardgameId
                    });
                }
                sellers.Add(sellerToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedSeller, sellerToAdd.Name, sellerToAdd.BoardgamesSellers.Count));
            }
            context.Sellers.AddRange(sellers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
