using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    [XmlType("Property")]
    public class ExportLargerPropertyDto
    {
        [Required]
        [XmlElement("PropertyIdentifier")]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; }
        [Required]
        [XmlElement("Area")]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }             
        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; }
        [Required]
        [XmlAttribute("postal-code")]
        public string PostalCode { get; set; }
    }

}
