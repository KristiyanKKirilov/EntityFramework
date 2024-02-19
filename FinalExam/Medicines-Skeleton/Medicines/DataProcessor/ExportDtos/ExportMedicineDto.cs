using Medicines.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ExportPharmacyDto Pharmacy { get; set; }
    }
}
