using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesAPI.Models;
using NotesAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class NotesController : ControllerBase
    {
        
        INoteCollectionService _noteCollectionService;
        public NotesController(INoteCollectionService noteCollectionService)
        {
            _noteCollectionService = noteCollectionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotes()
        {
            List<Notes> notes = await _noteCollectionService.GetAll();
            return Ok(notes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotes([FromBody] Notes note)
        {
            if (note == null)
            {
                return BadRequest("Note cannot be null");
            }
           
            if(await _noteCollectionService.Create(note))
            {
                return CreatedAtRoute("GetByNoteId", new { id = note.Id.ToString() }, note);
            }
            return NoContent();
            //return StatusCode(StatusCodes.Status500InternalServerError, "Error in processing the note");
        }

        [HttpGet("/id", Name = "GetByNoteId")]
        public async Task<IActionResult> GetByNoteId(Guid id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var note = await _noteCollectionService.Get(id);

            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet("OwnerId/{id}")]
        public async Task<IActionResult> GetByOwnerId(Guid id)
        {

            if (id == null)
            {
                return BadRequest();
            }

            var note = await _noteCollectionService.GetNotesByOwnerId(id);

            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {

            if (id == null)
            {
                return BadRequest();
            }

           
           bool isRemoved = await _noteCollectionService.Delete(id);
       
            if (!isRemoved)
            {
                return Ok();
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] Notes noteToUpdate)
        {
            if (noteToUpdate == null)
            {
                return BadRequest("Note cannot be null");
            }

            bool isUpdated = await _noteCollectionService.Update(id, noteToUpdate);
            if (isUpdated)
            {
                return Ok();
            }
            return NoContent();
        
        }


    }
}
