using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoogleKeepNotes.Model;
using GoogleKeepNotes.Models;

namespace GoogleKeepNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;

        public NotesController(NoteContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetNote([FromQuery] string Label, [FromQuery] string Pinned, [FromQuery] string Title)
        {

            return _context.Note.Include(x => x.labels).Include(x => x.checklists).Where(n =>
               n.Pinned.ToString() == ((Pinned == null) ? n.Pinned.ToString():Pinned) &&
               n.Title == (Title ?? n.Title) &&
               n.labels.Any(x => (Label != null)? x.label == Label : true)
               ).ToList();
        }

        // GET: api/Notes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var note = await _context.Note.FirstAsync(id) ;

            var note =await _context.Note.Include(x => x.labels).Include(x => x.checklists).SingleOrDefaultAsync(x => x.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
            //return Ok(_context.Note.Include(x=>x.labels).Include(x => x.checklists).SingleOrDefault( x => x.Id == id));
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.Id)
            {
                return BadRequest();
            }

            //Note n = await _context.Note.Include(x => x.labels).Include(x => x.checklists).SingleOrDefaultAsync(x => x.Id == id);
            //n.Pinned = note.Pinned;
            //n.Title = note.Title;
            //n.PlainText = note.PlainText;

            //List<Labels> l = new List<Labels>(note.labels);
            //List<CheckList> c = new List<CheckList>(note.checklists);

            await _context.Note.Include(x => x.labels).Include(x => x.checklists).ForEachAsync(x =>
            {
                if(x.Id == note.Id)
                {
                    x.Title = note.Title;
                    x.PlainText = note.PlainText;
                    x.Pinned = note.Pinned;
                    
                    foreach(Labels templ in note.labels)
                    {
                        Labels a = x.labels.Find(y => y.Id == templ.Id);
                        if(a != null)
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
                            CheckList ch = new CheckList() { checkList = tempc.checkList , isChecked = tempc.isChecked };
                            x.checklists.Add(ch);
                        }
                    }

                }
            });


            //_context.Entry(n).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetNote), new
            {
                note.Id,
                note
            });
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery] int id, [FromQuery] string Label, [FromQuery] string Pinned, [FromQuery] string Title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var note = await _context.Note.FindAsync(id);

            var note = _context.Note.Include(x => x.labels).Include(x => x.checklists).Where(n =>
                n.Pinned.ToString() == ((Pinned == null) ? n.Pinned.ToString() : Pinned) &&
                n.Title == (Title ?? n.Title) &&
                n.Id == ((id == 0) ? n.Id : id)
               //n.labels.Any(x => (Label != null) ? x.label == Label : true)
               ).ToList();

            if (note == null)
            {
                return NotFound();
            }

            //note.labels.Clear();
            //note.checklists.Clear();

            //var notes = _context.Note.Where(x => x.Id == id).ToList();
            note.ForEach(x => _context.Labels.RemoveRange(x.labels));
            note.ForEach(x => _context.CheckList.RemoveRange(x.checklists));
            _context.Note.RemoveRange(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}