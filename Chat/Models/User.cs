using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public Guid Password { get; set; }

        public DateTime? DateOfBirth { get; set; }
        [Required]
        public DateTime DateRegister { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; }

        public bool Hide { get; set; }
    }
}