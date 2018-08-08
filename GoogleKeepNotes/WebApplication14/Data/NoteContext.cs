using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication14.Models
{
    public class NoteContext : DbContext
    {
        public NoteContext (DbContextOptions<NoteContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication14.Models.Note> Note { get; set; }
        public DbSet<WebApplication14.Models.Labels> Labels { get; set; }
        public DbSet<WebApplication14.Models.CheckList> CheckList { get; set; }
    }
}
