namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            var serializer = new XmlSerializer(typeof(List<ImportDistrictDto>), new XmlRootAttribute("Districts"));
            var districtDtos = (List<ImportDistrictDto>)serializer.Deserialize(new StringReader(xmlDocument));
            List<District> districts = new();
            StringBuilder sb = new();
            var propIdentifiers = dbContext.Properties
                .Select(p => p.PropertyIdentifier).ToList();
            var addresses = dbContext.Properties
                .Select(p => p.Address).ToList();

            foreach (var districtDto in districtDtos)
            {
                if (!IsValid(districtDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                District districtToAdd = new District()
                {
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                    Region = (Region)Enum.Parse(typeof(Region), districtDto.Region)
                };

                if (districts.Contains(districtToAdd))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var property in districtDto.Properties)
                {
                    if (!IsValid(property))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime dateOfAcquisition;

                    if (!DateTime.TryParseExact(property.DateOfAcquisition, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out dateOfAcquisition))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (districtToAdd.Properties.Any(p => p.PropertyIdentifier == property.PropertyIdentifier)
                        || propIdentifiers.Contains(property.PropertyIdentifier))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (districtToAdd.Properties.Any(p => p.Address == property.Address)
                       || addresses.Contains(property.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property propertyToAdd = new()
                    {
                        PropertyIdentifier = property.PropertyIdentifier,
                        Area = property.Area,
                        Details = property.Details,
                        Address = property.Address,
                        DateOfAcquisition = dateOfAcquisition
                    };

                    districtToAdd.Properties.Add(propertyToAdd);
                }

                districts.Add(districtToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, districtToAdd.Name, districtToAdd.Properties.Count()));
            }

            dbContext.Districts.AddRange(districts);
            dbContext.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            var citizenDtos = JsonConvert.DeserializeObject<List<ImportCitizenDto>>(jsonDocument);
            List<Citizen> citizens = new();
            StringBuilder sb = new();
            var propertiesIds = dbContext.Properties.Select(p => p.Id).ToList();

            foreach (var citizenDto in citizenDtos)
            {
                if (!IsValid(citizenDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (citizenDto.MaritalStatus != "Unmarried" && citizenDto.MaritalStatus != "Married" && citizenDto.MaritalStatus != "Divorced"
                    && citizenDto.MaritalStatus != "Widowed")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime birthDate;

                if (!DateTime.TryParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None,
                         out birthDate))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen citizenToAdd = new()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = birthDate,
                    MaritalStatus = (MaritalStatus)Enum.Parse(typeof(MaritalStatus), citizenDto.MaritalStatus)
                };

                foreach (var propId in citizenDto.Properties.Distinct())
                {
                    if(!propertiesIds.Contains(propId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    citizenToAdd.PropertiesCitizens.Add(new PropertyCitizen()
                    {
                        PropertyId = propId
                    });
                    
                }

                citizens.Add(citizenToAdd);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen, citizenToAdd.FirstName, citizenToAdd.LastName,
                    citizenToAdd.PropertiesCitizens.Count));
            }
            dbContext.Citizens.AddRange(citizens);
            dbContext.SaveChanges();

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
