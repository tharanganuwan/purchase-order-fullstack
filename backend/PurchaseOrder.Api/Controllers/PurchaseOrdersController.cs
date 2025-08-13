using Microsoft.AspNetCore.Mvc;
using PurchaseOrder.Application.DTOs;
using PurchaseOrder.Application.Interfaces;
using PurchaseOrder.Application.Models;

namespace PurchaseOrder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        public PurchaseOrdersController(IPurchaseOrderService service) => _service = service;



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PoQueryParameters q)
        {
            var result = await _service.GetListAsync(q);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var po = await _service.GetByIdAsync(id);
            if (po == null) return NotFound();
            return Ok(po);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePoDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] CreatePoDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
