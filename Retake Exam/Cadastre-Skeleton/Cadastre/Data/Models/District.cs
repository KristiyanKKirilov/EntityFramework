using Cadastre.Data.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.Data.Models
{
    public class District
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        [RegularExpression("^[A-Z]{2}-\\d{5}$")]
        public string PostalCode { get; set; }
        [Required]
        public Region Region { get; set; }
        public virtual ICollection<Property> Properties { get; set; } = new HashSet<Property>();
    }
}
