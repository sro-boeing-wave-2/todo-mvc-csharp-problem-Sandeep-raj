using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;

namespace WebApplication14.Models
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string PlainText { get; set; }
        public bool Pinned { get; set; }
        public List<CheckList> checklists { get; set; }
        public List<Labels> labels { get; set; }

        public bool IsEquals(Note n)
        {

            if (Title == n.Title && PlainText == n.PlainText && Pinned == n.Pinned && labels.All(x => n.labels.Exists( y => y.label == x.label)) && checklists.All(x => n.checklists.Exists(y => (y.checkList == x.checkList && y.isChecked == x.isChecked)) ))
                return true;

            return false;

            
        }

    }

    public class Labels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String label { get; set; }

        //public bool IsEquals(Labels l)
        //{
        //    if (Id == l.Id && label == l.label)
        //        return true;
        //    return false;
        //}
    }

    public class CheckList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool isChecked { get; set; }
        public string checkList { get; set; }

        //public bool IsEquals(CheckList c)
        //{
        //    if (Id == c.Id && checkList == c.checkList && isChecked == c.isChecked)
        //        return true;
        //    return false;
        //}
    }
}
