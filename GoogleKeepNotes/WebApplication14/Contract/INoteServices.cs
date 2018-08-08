using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication14.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication14.Contract
{
    public interface INoteServices
    {
        IEnumerable<Note> getnote(int id, string Label,  string Pinned,  string Title);
        Task<Note> getnote( int id);
        Task putnote(Note note);
        Task postnote( Note note);
        Task<IEnumerable<Note>> deletenote( int id,  string Label,  string Pinned,  string Title);
    }
}
