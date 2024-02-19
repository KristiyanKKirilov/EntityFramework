using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportMedicineDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Name { get; set; }
        [Required]
        [Range(0.01, 1000.00)]
        public string Price { get; set; }
        [Required]
        public ExportMedicinePharmacyDto Pharmacy { get; set; }
    }
}
