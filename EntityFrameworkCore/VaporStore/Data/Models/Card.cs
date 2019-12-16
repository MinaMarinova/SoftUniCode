using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enum;

namespace VaporStore.Data.Models
{
    public class Card
    {
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^\b[0-9]{4}\b \b[0-9]{4}\b \b[0-9]{4}\b \b[0-9]{4}\b$")]
        public string Number { get; set; }

        [Required]
        [RegularExpression(@"^\b[0-9]{3}\b$")]
        public string Cvc { get; set; }

        [Required]
        public CardType Type { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; }

        public ICollection<Purchase> Purchases { get; set; } = new HashSet<Purchase>();
    }
}
