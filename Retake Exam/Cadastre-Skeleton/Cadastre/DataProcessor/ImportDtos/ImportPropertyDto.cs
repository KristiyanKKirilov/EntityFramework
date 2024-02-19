using Cadastre.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("Property")]
    public class ImportPropertyDto
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
        [XmlElement("Details")]
        [MinLength(5)]
        [MaxLength(500)]
        public string Details { get; set; }
        [Required]
        [XmlElement("Address")]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; }
    }
       
}
