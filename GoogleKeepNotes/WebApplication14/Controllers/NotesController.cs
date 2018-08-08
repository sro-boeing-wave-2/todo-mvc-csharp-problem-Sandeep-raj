using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication14.Models;
using WebApplication14.Services;
using WebApplication14.Contract;

namespace WebApplication14.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteContext _context;
        private INoteServices _services;

        public NotesController(NoteContext context)
        {
            _services = new NoteServices(context);
            _context = context;
        }


        // GET: api/Notes
        [HttpGet]
        public IActionResult GetNote([FromQuery] int Id,[FromQuery] string Label, [FromQuery] string Pinned, [FromQuery] string Title)
        {

            IEnumerable<Note> notes = _services.getnote(Id,Label, Pinned, Title);

            if(notes.Count() == 0)
            {
                return NotFound();
            }

            return Ok(notes);
        }

        //GET: api/Notes/5
        [HttpGet("{id}")]
        public IActionResult GetNoteById( int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = _services.getnote(id);

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
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

            try
            {
                await _services.putnote(note);
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

            return CreatedAtAction(nameof(GetNote), note);

        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _services.postnote(note);

            return CreatedAtAction(nameof(GetNote),note);
        }

        // DELETE: api/Notes/5
        [HttpDelete]
        public async Task<IActionResult> DeleteNote([FromQuery] int id, [FromQuery] string Label, [FromQuery] string Pinned, [FromQuery] string Title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var note = await _services.deletenote(id,Label,Pinned,Title);
            if(note == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }
    }
}