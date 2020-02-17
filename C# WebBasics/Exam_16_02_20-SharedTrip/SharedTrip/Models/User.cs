namespace SharedTrip.Models
{
    using SIS.MvcFramework;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserTrips = new HashSet<UserTrip>();
        }
        public string Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<UserTrip> UserTrips { get; set; }

        public IdentityRole Role { get; set; }
    }
}
