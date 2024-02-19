using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class ExportCreatorBoardgameDto
    {
        [Required]
        [XmlElement("BoardgameName")]
        [MinLength(10)]
        [MaxLength(20)]
        public string BoardgameName { get; set; }

        [Required]
        [XmlElement("BoardgameYearPublished")]
        [Range(2018, 2023)]
        public int BoardgameYearPublished { get; set; }

    }
}
