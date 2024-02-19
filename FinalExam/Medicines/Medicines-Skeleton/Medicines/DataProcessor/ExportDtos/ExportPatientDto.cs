using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientDto
    {
        [Required]
        [XmlAttribute("Gender")]
        public string Gender { get; set; }        
        [Required]
        [XmlElement("Name")]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        [XmlElement("AgeGroup")]
        public string AgeGroup { get; set; }
        [Required]
        [XmlArray("Medicines")] 
        public ExportPatientMedicineDto[] Medicines { get; set; }
    }
}
