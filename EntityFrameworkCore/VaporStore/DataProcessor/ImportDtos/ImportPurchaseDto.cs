using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ImportDtos
{
    [XmlType("Purchase")]
    public class ImportPurchaseDto
    {
        [XmlAttribute("title")]
        [Required]
        public string GameName { get; set; }

        [XmlElement("Type")]
        [Required]
        public string Type { get; set; }

        [XmlElement("Key")]
        [Required]
        [RegularExpression(@"^\b[0-9A-Z]{4}\b-\b[0-9A-Z]{4}\b-\b[0-9A-Z]{4}\b$")]
        public string ProductKey { get; set; }

        [XmlElement("Card")]
        [Required]
        public string CardNumber { get; set; }

        [XmlElement("Date")]
        [Required]
        public string Date { get; set; }
    }
}
