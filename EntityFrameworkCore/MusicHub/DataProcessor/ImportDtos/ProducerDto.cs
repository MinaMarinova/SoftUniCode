using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MusicHub.DataProcessor.ImportDtos
{
    public class ProducerDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string Pseudonym { get; set; }

        [RegularExpression(@"^\+359 \b[0-9]{3}\b \b[0-9]{3}\b \b[0-9]{3}\b$")]
        public string PhoneNumber { get; set; }

        public ICollection<AlbumDto> Albums { get; set; } = new HashSet<AlbumDto>();
    }
}
