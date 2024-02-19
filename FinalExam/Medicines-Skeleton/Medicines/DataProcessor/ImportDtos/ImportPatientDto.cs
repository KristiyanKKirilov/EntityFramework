using Medicines.Data.Models.Enums;
using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

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

//•	Id – integer, Primary Key
//•	FullName – text with length [5, 100] (required)
//•	AgeGroup – AgeGroup enum (Child = 0, Adult, Senior)(required)
//•	Gender – Gender enum (Male = 0, Female)(required)
//•	PatientsMedicines - collection of type PatientMedicine