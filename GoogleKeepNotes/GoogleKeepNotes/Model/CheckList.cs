using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleKeepNotes.Model
{
    public class CheckList
    {
        public int Id { get; set; }
        public bool isChecked { get; set; }
        public string checkList { get; set; }
    }
}
