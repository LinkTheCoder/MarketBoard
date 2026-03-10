using Microsoft.AspNetCore.Mvc;
using SubscribersWebAPI.DTOs;
using SubscribersWebAPI.Models;
using SubscribersWebAPI.Services;

namespace SubscribersWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisementsController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementsController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        /// <summary>
        /// Skapar en ny annons
        /// </summary>
        /// <param name="createDto">Annonsdata</param>
        /// <returns>Den skapade annonsen</returns>
        [HttpPost]
        [ProducesResponseType(typeof(AdvertisementDto), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AdvertisementDto>> CreateAdvertisement(CreateAdvertisementDto createDto)
        {
            try
            {
                var advertisement = await _advertisementService.CreateAdvertisementAsync(createDto);
                return CreatedAtAction(
                    nameof(GetAdvertisementById), 
                    new { id = advertisement.Id }, 
                    advertisement);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Hämtar alla publicerade annonser
        /// </summary>
        /// <returns>Lista över publicerade annonser</returns>
        [HttpGet("published")]
        [ProducesResponseType(typeof(IEnumerable<AdvertisementDto>), 200)]
        public async Task<ActionResult<IEnumerable<AdvertisementDto>>> GetPublishedAdvertisements()
        {
            var advertisements = await _advertisementService.GetPublishedAdvertisementsAsync();
            return Ok(advertisements);
        }

        /// <summary>
        /// Hämtar alla annonser (admin)
        /// </summary>
        /// <returns>Lista över alla annonser</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AdvertisementDto>), 200)]
        public async Task<ActionResult<IEnumerable<AdvertisementDto>>> GetAllAdvertisements()
        {
            var advertisements = await _advertisementService.GetAllAdvertisementsAsync();
            return Ok(advertisements);
        }

        /// <summary>
        /// Hämtar en specifik annons
        /// </summary>
        /// <param name="id">Annons-ID</param>
        /// <returns>Annonsen</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AdvertisementDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<AdvertisementDto>> GetAdvertisementById(int id)
        {
            var advertisement = await _advertisementService.GetAdvertisementByIdAsync(id);
            
            if (advertisement == null)
            {
                return NotFound($"Ingen annons hittades med ID: {id}");
            }

            return Ok(advertisement);
        }

        /// <summary>
        /// Hämtar annonser för en specifik prenumerant
        /// </summary>
        /// <param name="subscriptionNumber">Prenumerationsnummer</param>
        /// <returns>Lista över prenumerantens annonser</returns>
        [HttpGet("subscriber/{subscriptionNumber}")]
        [ProducesResponseType(typeof(IEnumerable<AdvertisementDto>), 200)]
        public async Task<ActionResult<IEnumerable<AdvertisementDto>>> GetAdvertisementsBySubscriber(string subscriptionNumber)
        {
            var advertisements = await _advertisementService.GetAdvertisementsBySubscriberAsync(subscriptionNumber);
            return Ok(advertisements);
        }

        /// <summary>
        /// Uppdaterar status för en annons
        /// </summary>
        /// <param name="id">Annons-ID</param>
        /// <param name="status">Ny status</param>
        /// <returns>Bekräftelse</returns>
        [HttpPatch("{id}/status")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateAdvertisementStatus(int id, [FromBody] Models.AdvertisementStatus status)
        {
            var result = await _advertisementService.UpdateAdvertisementStatusAsync(id, status);
            
            if (!result)
            {
                return NotFound($"Ingen annons hittades med ID: {id}");
            }

            return NoContent();
        }

        /// <summary>
        /// Tar bort en annons
        /// </summary>
        /// <param name="id">Annons-ID</param>
        /// <returns>Bekräftelse</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteAdvertisement(int id)
        {
            var result = await _advertisementService.DeleteAdvertisementAsync(id);
            
            if (!result)
            {
                return NotFound($"Ingen annons hittades med ID: {id}");
            }

            return NoContent();
        }

        /// <summary>
        /// Beräknar annonspris för olika användarkategorier
        /// </summary>
        /// <param name="advertiserType">Typ av annonsör</param>
        /// <returns>Beräknat pris</returns>
        [HttpGet("calculate-price/{advertiserType}")]
        [ProducesResponseType(typeof(decimal), 200)]
        public ActionResult<decimal> CalculatePrice(Models.AdvertiserType advertiserType)
        {
            var price = _advertisementService.CalculateAdvertisementPrice(advertiserType);
            return Ok(price);
        }
    }
}