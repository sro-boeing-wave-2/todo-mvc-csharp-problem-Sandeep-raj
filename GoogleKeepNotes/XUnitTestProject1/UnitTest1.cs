using System;
using Xunit;
using GoogleKeepNotes.Controllers;
using GoogleKeepNotes.Model;
using System.Collections.Generic;
using GoogleKeepNotes.Models;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            public void GetAllProducts_ShouldReturnAllProducts()
            {
                var testNote = GetTestProducts();
                var controller = new NotesController(testNote);

                var result = controller.GetAllProducts() as List<Product>;
                Assert.AreEqual(testProducts.Count, result.Count);
            }
        }

        private List<Note> GetTestProducts()
        {
            var testNote = new List<Note>();
            List<CheckList> c = new List<CheckList>();
            List<Labels> l = new List<Labels>();
            l.Add(new Labels() { label = "Heey" });
            c.Add(new CheckList() { checkList = "Hello", isChecked = true });
            testNote.Add(new Note { Title = "First Title", PlainText = "Hello World", Pinned = true, checklists = c, labels = l });
            return testNote;
        }

        public class NoteContext : DbContext
        {
            public NoteContext(DbContextOptions<NoteContext> options)
                : base(options)
            {
            }

            public DbSet<GoogleKeepNotes.Model.Note> Note { get; set; }
            public DbSet<GoogleKeepNotes.Model.Labels> Labels { get; set; }
            public DbSet<GoogleKeepNotes.Model.CheckList> CheckList { get; set; }
        }
    }
}
