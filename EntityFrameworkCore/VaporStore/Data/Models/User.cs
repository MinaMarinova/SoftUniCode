using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][A-Za-z]+ [A-Z][A-Za-z]+$")]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(3, 103)]
        public int Age { get; set; }

        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();



    }
}
