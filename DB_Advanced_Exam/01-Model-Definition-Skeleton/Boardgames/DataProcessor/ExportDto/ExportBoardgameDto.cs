using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    public class ExportBoardgameDto
    {
        [Required]
        [MinLength(10)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [Range(1, 10.00)]
        public double Rating { get; set; }      
        
        [Required]
        public string Mechanics { get; set; }
        [Required]
        public string Category { get; set; }
    }
}
