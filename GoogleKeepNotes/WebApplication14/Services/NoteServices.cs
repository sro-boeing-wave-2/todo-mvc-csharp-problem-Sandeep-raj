using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication14.Contract;
using WebApplication14.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebApplication14.Services
{
    public class NoteServices : INoteServices
    {
        private static NoteContext _context;

        public NoteServices(NoteContext context)
        {
            _context = context;
        }

        public IEnumerable<Note> getnote(int id ,string Label,string Pinned,string Title)
        {
            return _context.Note.Include(x => x.labels).Include(x => x.checklists).Where(n =>
               n.Pinned == ((Pinned == null) ? n.Pinned : Convert.ToBoolean(Pinned)) &&
               n.Title == (Title ?? n.Title) &&
               n.labels.Any(x => (Label != null) ? x.label == Label : true) &&
               n.Id == ((id == 0) ? n.Id : id)
               ).ToList();
        }

        public async Task<Note> getnote(int id)
        {
            return await _context.Note.Include(x => x.labels).Include(x => x.checklists).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task putnote(Note note)
        {
            await _context.Note.Include(x => x.labels).Include(x => x.checklists).ForEachAsync(x =>
            {
                if (x.Id == note.Id)
                {
                    x.Title = note.Title;
                    x.PlainText = note.PlainText;
                    x.Pinned = note.Pinned;

                    foreach (Labels templ in note.labels)
                    {
                        Labels a = x.labels.Find(y => y.Id == templ.Id);
                        if (a != null)
                        {
                            a.label = templ.label;
                        }
                        else
                        {
                            Labels lab = new Labels() { label = templ.label };
                            x.labels.Add(lab);
                        }
                    }

                    foreach (CheckList tempc in note.checklists)
                    {
                        CheckList a = x.checklists.Find(y => y.Id == tempc.Id);
                        if (a != null)
                        {
                            a.checkList = tempc.checkList;
                            a.isChecked = tempc.isChecked;
                        }
                        else
                        {
                            CheckList ch = new CheckList() { checkList = tempc.checkList, isChecked = tempc.isChecked };
                            x.checklists.Add(ch);
                        }
                    }

                }
            });
            await _context.SaveChangesAsync();
        }

        public async Task postnote(Note note)
        {
            _context.Note.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Note>> deletenote(int id, string Label,  string Pinned, string Title)
        {
            var note = _context.Note.Include(x => x.labels).Include(x => x.checklists).Where(n =>
                n.Pinned == ((Pinned == null) ? n.Pinned : Convert.ToBoolean(Pinned)) &&
                n.Title == (Title ?? n.Title) &&
                n.Id == ((id == 0) ? n.Id : id) &&
                n.labels.Any(x => (Label != null) ? x.label == Label : true)
               ).ToList();

            if (note.Count() == 0)
            {
                return null;
            }

            note.ForEach(x => _context.Labels.RemoveRange(x.labels));
            note.ForEach(x => _context.CheckList.RemoveRange(x.checklists));
            _context.Note.RemoveRange(note);
            await _context.SaveChangesAsync();

            return note;

        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}
