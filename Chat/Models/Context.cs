using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Chat.Models
{
    public class Context : DbContext
    {
        public Context():base("DefaultConnection")
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}