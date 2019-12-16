using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{   
    [XmlType("Prisoner")]
    public class PrisonerForOfficerDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
