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

    public class OwnerController : ControllerBase
    {
        IOwnerService _ownerService;
        public OwnerController(IOwnerService ownerService)
        {
            _ownerService = ownerService;
        }
        [HttpGet]
        public async Task<IActionResult> GetOwners()
        {
            List<Owner> owners = await _ownerService.GetAll();
            return Ok(owners);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOwners([FromBody] Owner owner)
        {
            if (owner == null)
            {
                return BadRequest("Owner cannot be null");
            }

            if (await _ownerService.Create(owner))
            {
                return CreatedAtRoute("GetByOwnerId", new { id = owner.Id.ToString() }, owner);
            }
            return NoContent();
            //return StatusCode(StatusCodes.Status500InternalServerError, "Error in processing the note");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOwner(Guid id, [FromBody] Owner ownerToUpdate)
        {
            if (ownerToUpdate == null)
            {
                return BadRequest("Owner cannot be null");
            }

            bool isUpdated = await _ownerService.Update(id, ownerToUpdate);
            if (isUpdated)
            {
                return Ok();
            }
            return NoContent();

        }
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {

            if (id == null)
            {
                return BadRequest();
            }


            bool isRemoved = await _ownerService.Delete(id);

            if (!isRemoved)
            {
                return Ok();
            }

            return NotFound();
        }
        [HttpGet("Id/{id}")]
        public async Task<IActionResult> GetByOwnerId(Guid id)
        {

            if (id == null)
            {
                return BadRequest();
            }

            var owner = await _ownerService.GetOwnersById(id);

            if (owner == null)
            {
                return NotFound();
            }
            return Ok(owner);
        }

    }
}
