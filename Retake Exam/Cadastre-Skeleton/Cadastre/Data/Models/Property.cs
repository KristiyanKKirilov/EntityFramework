using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier  { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }
        [MinLength(5)]
        [MaxLength(500)]
        public string Details { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public DateTime DateOfAcquisition  { get; set; }
        [Required]
        public int DistrictId  { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public District District { get; set; }
        public virtual ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new HashSet<PropertyCitizen>();
    }
}
