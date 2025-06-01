using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Merch_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchController(IMerchService merchService) : ControllerBase
    {
        private readonly IMerchService _merchService = merchService;



        [HttpPost]
        public async Task<IActionResult> Create(CreateMerch merch)
        {
            if (!ModelState.IsValid)
                return BadRequest(merch);

            var result = await _merchService.CreateAsync(merch);
            if (result == null)
                return BadRequest("Failed to create merch.");

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var merch = await _merchService.GetAllAsync();
            return Ok(merch);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var merch = await _merchService.GetByIdAsync(id);
            if (merch == null)
                return NotFound();

            return Ok(merch);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateMerch merch)
        {
            if (!ModelState.IsValid)
                return BadRequest(merch);

            var updated = await _merchService.UpdateAsync(id, merch);
            if (!updated)
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _merchService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}
