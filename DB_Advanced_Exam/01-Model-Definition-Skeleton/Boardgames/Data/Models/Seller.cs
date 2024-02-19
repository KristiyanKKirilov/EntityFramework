using System.ComponentModel.DataAnnotations;

namespace Boardgames.Data.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Address { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        [RegularExpression("^www\\.[A-Za-z0-9\\-]+\\.com$")]
        public string Website { get; set; }
        public virtual ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();

    }
}
