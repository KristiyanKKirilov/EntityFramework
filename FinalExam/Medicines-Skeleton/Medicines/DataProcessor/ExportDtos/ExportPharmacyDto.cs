using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportPharmacyDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        [RegularExpression(@"^\([0-9]{3}\) [0-9]{3}\-[0-9]{4}$")]
        public string PhoneNumber { get; set; }

    }
}
