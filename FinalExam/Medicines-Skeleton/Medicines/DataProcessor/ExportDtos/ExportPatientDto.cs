using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class ExportPatientDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]       
        public string AgeGroup { get; set; }
        [Required]
        [XmlAttribute("Gender")]       
        public string Gender { get; set; }
        [XmlArray("Medicines")]
        public ExportMedicinePatientDto[] Medicines { get; set; }
    }
}
