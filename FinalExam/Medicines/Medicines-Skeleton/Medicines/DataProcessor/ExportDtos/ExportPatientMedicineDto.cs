using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Medicine")]
    public class ExportPatientMedicineDto
    {
        [Required]
        [XmlElement("Name")]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 1000.00)]
        [XmlElement("Price")]
        public string Price { get; set; }
        [Required]
        [XmlAttribute("Category")]
        public string Category { get; set; }        
        [Required]
        [XmlElement("Producer")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer { get; set; }
        [Required]
        [XmlElement("BestBefore")]
        public string ExpiryDate { get; set; }
        
    }
}
