using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GoogleKeepNotes.Model;

namespace GoogleKeepNotes.Models
{
    public class NoteContext : DbContext
    {
        public NoteContext (DbContextOptions<NoteContext> options)
            : base(options)
        {
        }

        public DbSet<GoogleKeepNotes.Model.Note> Note { get; set; }
        public DbSet<GoogleKeepNotes.Model.Labels> Labels { get; set; }
        public DbSet<GoogleKeepNotes.Model.CheckList> CheckList { get; set; }
    }
}
