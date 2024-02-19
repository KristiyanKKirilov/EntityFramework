using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {     
        [Required]
        [XmlElement("Name")]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [XmlElement("PostalCode")]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression("^[A-Z]{2}-\\d{5}$")]
        public string PostalCode { get; set; }
        [Required]
        [XmlAttribute("Region")]
        public string Region { get; set; }
        [XmlArray("Properties")]
        public ImportPropertyDto[] Properties { get; set; }
    }
}
