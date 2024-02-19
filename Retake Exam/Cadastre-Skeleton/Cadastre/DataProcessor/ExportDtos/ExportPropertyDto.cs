using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropertyDto
    {
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        public string PropertyIdentifier { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }
        [Required]   
        [MinLength(5)]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]        
        public string DateOfAcquisition { get; set; }
        [Required]
        public ExportOwnerDto[] Owners { get; set; }
    }
}
