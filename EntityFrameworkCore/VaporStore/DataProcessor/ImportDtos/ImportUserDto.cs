using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.DataProcessor.ImportDtos
{
    public class ImportUserDto
    {
        [Required]
        [RegularExpression(@"^[A-Z][A-Za-z]+ [A-Z][A-Za-z]+$")]
        public string FullName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }
        
        [Required]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public List<ImportCardDto> Cards { get; set; } = new List<ImportCardDto>();
    }
}
