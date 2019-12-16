using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ImportDtos
{
    public class ImportCardDto
    {
        [Required]
        [RegularExpression(@"^\b[0-9]{4}\b \b[0-9]{4}\b \b[0-9]{4}\b \b[0-9]{4}\b$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^\b[0-9]{3}\b$")]
        public string CVC { get; set; }
        
        [Required]
        public string Type { get; set; }
    }
}
