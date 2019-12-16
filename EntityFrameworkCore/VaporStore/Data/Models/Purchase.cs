using System;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enum;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required]
        public PurchaseType Type { get; set; }

        [Required]
        [RegularExpression(@"^\b[0-9A-Z]{4}\b-\b[0-9A-Z]{4}\b-\b[0-9A-Z]{4}\b$")]
        public string ProductKey { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int CardId { get; set; }

        [Required]
        public Card Card { get; set; }
        
        public int GameId  { get; set; }

        [Required]
        public Game Game { get; set; }
    }
}
