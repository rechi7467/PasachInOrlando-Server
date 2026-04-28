using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrlandoServices.Core.DTOs;
using OrlandoServices.Core.Exceptions;
using OrlandoServices.Core.Interfaces.Service;
using OrlandoServices.Core.Models.Enums;

namespace OrlandoServices.Controllers
{
    [ApiController]
    [Route("api/donations")]
    public class DonationsController : ControllerBase
    {
        private readonly IDonationService _donationService;

        public DonationsController(IDonationService donationService)
        {
            _donationService = donationService;
        }

        // לקוח — יצירת תרומה חדשה
        [HttpPost]
        public IActionResult Create([FromBody] CreateDonationDto dto)
        {
            try
            {
                var result = _donationService.CreateDonation(dto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // לקוח — תרומות של משתמש מסוים
        [Authorize]
        [HttpGet("user/{userId}")]
        public IActionResult GetByUser(int userId)
        {
            var donations = _donationService.GetDonationsByUser(userId);
            return Ok(donations);
        }

        // אדמין — כל התרומות, אופציונלית לפי סטטוס
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll([FromQuery] PaymentStatus? status)
        {
            if (status.HasValue)
                return Ok(_donationService.GetDonationsByStatus(status.Value));

            return Ok(_donationService.GetAll());
        }

        // אדמין / מערכת תשלום — עדכון סטטוס תרומה
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] PaymentStatus newStatus)
        {
            try
            {
                _donationService.UpdateDonationStatus(id, newStatus);
                return Ok("Donation status updated successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
