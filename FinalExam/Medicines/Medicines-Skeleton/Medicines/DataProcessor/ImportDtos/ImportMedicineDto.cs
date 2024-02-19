using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Medicine")]
    public class ImportMedicineDto
    {
        [Required]
        [XmlAttribute("category")]
        public int Category { get; set; }
        [Required]
        [XmlElement("Name")]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [XmlElement("Price")]
        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }
        [Required]
        [XmlElement("ProductionDate")]
        public string ProductionDate { get; set; }
        [Required]
        [XmlElement("ExpiryDate")]
        public string ExpiryDate { get; set; }
        [Required]
        [XmlElement("Producer")]
        [MinLength(3)]
        [MaxLength(100)]
        public string Producer { get; set; }
    }
}
