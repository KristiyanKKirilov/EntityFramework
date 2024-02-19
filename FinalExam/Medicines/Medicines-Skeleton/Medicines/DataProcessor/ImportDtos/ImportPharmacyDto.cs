using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmacyDto
    {
        [Required]
        [XmlAttribute("non-stop")]
        public string IsNonStop { get; set; }
        [Required]
        [XmlElement("Name")]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(@"^\([0-9]{3}\) [0-9]{3}\-[0-9]{4}$")]
        [MinLength(14)]
        [MaxLength(14)]
        public string PhoneNumber { get; set; }
        
        [XmlArray("Medicines")]
        public ImportMedicineDto[] Medicines { get; set; }

    }
}
