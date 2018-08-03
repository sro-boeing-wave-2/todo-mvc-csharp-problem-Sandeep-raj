using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleKeepNotes.Model
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PlainText { get; set; }
        public bool Pinned { get; set; }
        public List<CheckList> checklists { get; set; }
        public List<Labels> labels { get; set; }
    }
}
