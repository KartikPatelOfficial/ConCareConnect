using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathologyBackend.Models;
using System.Security.Claims;

namespace PathologyBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferenceController : ControllerBase
    {

        private readonly DiagnosticServiceContext _context;

        public ReferenceController(DiagnosticServiceContext context)
        {
            _context = context;
        }

        // POST: api/References
        [HttpPost]
        public async Task<IActionResult> AddReference(ReferenceRequest referenceRequest)
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest();
            }

            var reference = new Reference
            {
                Name = referenceRequest.Name,
                Laboratoryid = uid,
                Phone = referenceRequest.Phone,
                Cutoff = referenceRequest.Cutoff,
                Qualification = referenceRequest.Qualification,
                Address = referenceRequest.Address,
                Hospital = referenceRequest.Hospital,
                Createdat = DateTime.Now
            };

            await _context.References.AddAsync(reference);
            await _context.SaveChangesAsync();

            return Ok(reference);
        }

        // GET: api/References
        [HttpGet]
        public async Task<IActionResult> GetReferences()
        {
            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (uid == null)
            {
                return BadRequest();
            }

            var references = await _context.References.Where(r => r.Laboratoryid == uid).ToListAsync();

            return Ok(references);
        }

    }
}
