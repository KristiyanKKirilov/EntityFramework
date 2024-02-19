using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ExportDto
{
    public class ExportSellerDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; }
     
        
        [Required]
        [RegularExpression("^www\\.[A-Za-z0-9\\-]+\\.com$")]
        public string Website { get; set; }
        public ExportBoardgameDto[] Boardgames { get; set; }
    }
}
