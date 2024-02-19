using Medicines.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.Data
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(14)]
        [MaxLength(14)]
        [RegularExpression(@"^\([0-9]{3}\) [0-9]{3}\-[0-9]{4}$")]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsNonStop { get; set; }

        public ICollection<Medicine> Medicines { get; set; } = new HashSet<Medicine>();
    }
}

//•	Id – integer, Primary Key
//•	Name – text with length [2, 50] (required)
//•	PhoneNumber – text with length 14. (required)
//o All phone numbers must have the following structure: three digits enclosed in parentheses, followed by a space, three more digits, a hyphen, and four final digits: 
//	Example-> (123) 456 - 7890
//•	IsNonStop – bool  (required)
//•	Medicines - collection of type Medicine
