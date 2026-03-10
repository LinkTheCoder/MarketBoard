using Microsoft.AspNetCore.Mvc;
using SubscribersWebAPI.DTOs;
using SubscribersWebAPI.Models;
using SubscribersWebAPI.Services;

namespace SubscribersWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscribersController : ControllerBase
    {
        private readonly ISubscriberService _subscriberService;

        public SubscribersController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }

        /// <summary>
        /// Hämtar prenumerantinformation baserat på prenumerationsnummer
        /// Detta är huvudendpointen som annonssystemet kommer att använda
        /// </summary>
        /// <param name="subscriptionNumber">Prenumerationsnummer</param>
        /// <returns>Prenumerantinformation</returns>
        [HttpGet("by-subscription/{subscriptionNumber}")]
        [ProducesResponseType(typeof(SubscriberInfoDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SubscriberInfoDto>> GetSubscriberBySubscriptionNumber(string subscriptionNumber)
        {
            var subscriber = await _subscriberService.GetSubscriberByNumberAsync(subscriptionNumber);
            
            if (subscriber == null)
            {
                return NotFound($"Ingen prenumerant hittades med prenumerationsnummer: {subscriptionNumber}");
            }

            return Ok(subscriber);
        }

        /// <summary>
        /// Hämtar alla aktiva prenumeranter
        /// </summary>
        /// <returns>Lista över alla aktiva prenumeranter</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SubscriberInfoDto>), 200)]
        public async Task<ActionResult<IEnumerable<SubscriberInfoDto>>> GetAllActiveSubscribers()
        {
            var subscribers = await _subscriberService.GetAllActiveSubscribersAsync();
            return Ok(subscribers);
        }

        /// <summary>
        /// Hämtar en prenumerant baserat på ID
        /// </summary>
        /// <param name="id">Prenumerant-ID</param>
        /// <returns>Prenumerantinformation</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Subscriber), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Subscriber>> GetSubscriberById(int id)
        {
            var subscriber = await _subscriberService.GetSubscriberByIdAsync(id);
            
            if (subscriber == null)
            {
                return NotFound($"Ingen prenumerant hittades med ID: {id}");
            }

            return Ok(subscriber);
        }

        /// <summary>
        /// Skapar en ny prenumerant
        /// </summary>
        /// <param name="subscriberDto">Prenumerantdata</param>
        /// <returns>Den skapade prenumeranten</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Subscriber), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<Subscriber>> CreateSubscriber(CreateSubscriberDto subscriberDto)
        {
            // Kontrollera om prenumerationsnumret redan finns
            if (await _subscriberService.SubscriberExistsAsync(subscriberDto.SubscriptionNumber))
            {
                return BadRequest($"Prenumerationsnummer {subscriberDto.SubscriptionNumber} finns redan.");
            }

            // Konvertera DTO till Subscriber model (ID kommer automatiskt att vara 0)
            var subscriber = subscriberDto.ToSubscriber();

            var createdSubscriber = await _subscriberService.CreateSubscriberAsync(subscriber);
            
            return CreatedAtAction(
                nameof(GetSubscriberById), 
                new { id = createdSubscriber.Id }, 
                createdSubscriber);
        }

        /// <summary>
        /// Uppdaterar en befintlig prenumerant
        /// </summary>
        /// <param name="id">Prenumerant-ID</param>
        /// <param name="subscriber">Uppdaterad prenumerantdata</param>
        /// <returns>Den uppdaterade prenumeranten</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Subscriber), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Subscriber>> UpdateSubscriber(int id, Subscriber subscriber)
        {
            var updatedSubscriber = await _subscriberService.UpdateSubscriberAsync(id, subscriber);
            
            if (updatedSubscriber == null)
            {
                return NotFound($"Ingen prenumerant hittades med ID: {id}");
            }

            return Ok(updatedSubscriber);
        }

        /// <summary>
        /// Tar bort en prenumerant (mjuk borttagning - sätter IsActive till false)
        /// </summary>
        /// <param name="id">Prenumerant-ID</param>
        /// <returns>Bekräftelse på borttagning</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSubscriber(int id)
        {
            var result = await _subscriberService.DeleteSubscriberAsync(id);
            
            if (!result)
            {
                return NotFound($"Ingen prenumerant hittades med ID: {id}");
            }

            return NoContent();
        }

        /// <summary>
        /// Kontrollerar om ett prenumerationsnummer existerar
        /// </summary>
        /// <param name="subscriptionNumber">Prenumerationsnummer</param>
        /// <returns>True om prenumerationsnumret finns</returns>
        [HttpHead("by-subscription/{subscriptionNumber}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> CheckSubscriberExists(string subscriptionNumber)
        {
            var exists = await _subscriberService.SubscriberExistsAsync(subscriptionNumber);
            
            if (exists)
            {
                return Ok();
            }

            return NotFound();
        }
    }
}