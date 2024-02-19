using Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [Required]
        [XmlElement("FirstName")]
        [MinLength(2)]
        [MaxLength(7)]
        public string FirstName { get; set; }
        [Required]
        [XmlElement("LastName")]
        [MinLength(2)]
        [MaxLength(7)]
        public string LastName { get; set; }
        [XmlArray("Boardgames")]
        public ImportBoardgameDto[] Boardgames { get; set; }
    }
}
