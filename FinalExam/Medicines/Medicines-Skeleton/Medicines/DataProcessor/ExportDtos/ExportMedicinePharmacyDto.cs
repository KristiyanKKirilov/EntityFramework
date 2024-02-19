using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportMedicinePharmacyDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^\([0-9]{3}\) [0-9]{3}\-[0-9]{4}$")]
        [MinLength(14)]
        [MaxLength(14)]
        public string PhoneNumber { get; set; }
    }
}
