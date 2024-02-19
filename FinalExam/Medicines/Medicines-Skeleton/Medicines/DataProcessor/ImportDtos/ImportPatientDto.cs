using System.ComponentModel.DataAnnotations;

namespace Medicines.DataProcessor.ImportDtos
{
    public class ImportPatientDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        public int AgeGroup { get; set; }
        [Required]
        public int Gender { get; set; }
        public int[] Medicines { get; set; }
    }
}
