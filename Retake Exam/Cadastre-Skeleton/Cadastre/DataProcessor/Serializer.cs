using Cadastre.Data;
using Cadastre.DataProcessor.ExportDtos;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor
{
    public class Serializer
    {
        public static string ExportPropertiesWithOwners(CadastreContext dbContext)
        {
            string dateString = "01/01/2000";

            DateTime dateToComp = DateTime.ParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var properties = dbContext.Properties
                 .Where(p => p.DateOfAcquisition >= dateToComp)
                 .OrderByDescending(p => p.DateOfAcquisition)
                     .ThenBy(p => p.PropertyIdentifier)
                 .Select(p => new ExportPropertyDto()
                 {
                     PropertyIdentifier = p.PropertyIdentifier,
                     Area = p.Area,
                     Address = p.Address,
                     DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                     Owners = p.PropertiesCitizens
                     .OrderBy(pc => pc.Citizen.LastName)
                     .Select(pc => new ExportOwnerDto()
                     {
                         LastName = pc.Citizen.LastName,
                         MaritalStatus = pc.Citizen.MaritalStatus.ToString(),
                     }).ToArray()
                 }).ToArray();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }

        public static string ExportFilteredPropertiesWithDistrict(CadastreContext dbContext)
        {
            var serializer = new XmlSerializer(typeof(ExportLargerPropertyDto[]), new XmlRootAttribute("Properties"));
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new();
           

            var properties = dbContext.Properties
                .Where(p => p.Area >= 100)
                .OrderByDescending(p => p.Area)
                    .ThenBy(p => p.DateOfAcquisition)
                .Select(p => new ExportLargerPropertyDto()
                {
                    PropertyIdentifier = p.PropertyIdentifier,
                    Area = p.Area,
                    DateOfAcquisition = p.DateOfAcquisition.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    PostalCode = p.District.PostalCode,
                }).ToArray();

            using StringWriter sw = new StringWriter(sb);

            serializer.Serialize(sw, properties, namespaces);

            return sb.ToString().TrimEnd();

        }
    }
}
