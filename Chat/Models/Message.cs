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

        /// <summary>
        /// Автор сообщения
        /// </summary>
        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        /// <summary>
        /// Автор сообщения
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Получатель сообщения
        /// </summary>
        [ForeignKey("Recipient")]
        public int? RecipientId { get; set; }
        /// <summary>
        /// Получатель сообщения
        /// </summary>
        public User Recipient { get; set; }

        public DateTime Date { get; set; }
    }
}