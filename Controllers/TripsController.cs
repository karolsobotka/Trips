using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using trips.Models.DTO;
using trips.Services;

namespace trips.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TripsController : ControllerBase
	{
		private readonly IDatabaseService _dbService;

		public TripsController(IDatabaseService dbService)
		{
			_dbService = dbService;
		}

		[HttpGet]
		public async Task<IActionResult> GetTripsAsync()
		{
			return Ok(await _dbService.GetTrips());
		}

		[HttpDelete("/api/clients/{idClient}")]
		public async Task<IActionResult> DeletClient([FromRoute] int idClient)
		{
			if (await _dbService.IsClientAssigned(idClient))
				return BadRequest("Nie można usunąć klienta");

			await _dbService.DeleteClient(idClient);

			return Ok();
		}

		[HttpPost("{idTrip}/clients")]
		public async Task<IActionResult> AssignClientToTrip([FromRoute] int idTrip, [FromBody] AssignClientToTripDto client)
		{
			int idClient = await _dbService.AddClient(new()
			{
				FirstName = client.FirstName,
				LastName = client.LastName,
				Email = client.Email,
				Telephone = client.Telephone,
				Pesel = client.Pesel
			});

			if (await _dbService.IsClientAssignedToTrip(idClient, idTrip))
				return BadRequest("Client is already assigned to the trip");

			if (idTrip != client.IdTrip || !await _dbService.TripExists(client.IdTrip, client.TripName))
				return BadRequest("Invalid trip");

			await _dbService.AssignClient(client);

			return Ok();
		}
	}
}
