using System.Security.Claims;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PathologyBackend.Models;

namespace PathologyBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoriesController : ControllerBase
    {
        private readonly DiagnosticServiceContext _context;

        public LaboratoriesController(DiagnosticServiceContext context)
        {
            _context = context;
        }

        // POST: api/Laboratories
        [HttpPost]
        public async Task<ActionResult<Laboratory>> PostLaboratory(LaboratoryRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (LaboratoryExists(request.Email))
            {
                return BadRequest("Laboratory already exists");
            }

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
            {
                Email = request.Email,
                EmailVerified = true,
                Password = request.Password,
                DisplayName = request.Name,
                Disabled = false,
                PhoneNumber = request.Phone,
                PhotoUrl = request.Logo
            });

            await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(userRecord.Uid, new Dictionary<string, object>
            {
                { "role", "laboratory" }
            });

            Laboratory laboratory = new Laboratory
            {
                Name = request.Name,
                Email = request.Email,
                Logo = request.Logo,
                Phone = request.Phone,
                Createdat = DateTime.Now,
                Status = "ACTIVE",
                Sign = request.Sign,
                Addressline1 = request.AddressLine1,
                Addressline2 = request.AddressLine2,
                City = request.City,
                Pincode = request.Pincode,
                Firebaseuid = userRecord.Uid
            };

            _context.Laboratories.Add(laboratory);
            await _context.SaveChangesAsync();

            return Ok(laboratory);
        }

        // POST: api/Laboratories/login
        [Authorize]
        [HttpGet("login")]
        public async Task<ActionResult<Laboratory>> Login()
        {

            var uid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Laboratory? laboratory = await _context.Laboratories.FirstOrDefaultAsync(lab => lab.Firebaseuid == uid);

            if (laboratory == null)
            {
                return NotFound("Laboratory not found");
            }

            return Ok(laboratory);
        }

        private bool LaboratoryExists(string email)
        {
            return _context.Laboratories.Any(e => e.Email == email);
        }

    }
}
