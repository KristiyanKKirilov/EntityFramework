using Boardgames.DataProcessor.ImportDto;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [Required]
        [XmlElement("CreatorName")]        
        public string CreatorName { get; set; }
        [Required]
        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        [XmlArray("Boardgames")]
        public ExportCreatorBoardgameDto[] Boardgames { get; set; }
    }
}
