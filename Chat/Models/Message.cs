using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Message
    {
        public int Id { get; set; }
        [MaxLength(1000)]
        [Display(Name ="Текст сообщения")]
        public string Text { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        [ForeignKey("Recipient")]
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
    }
}