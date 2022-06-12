using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using trips.Models;
using trips.Models.DTO;

namespace trips.Services
{
	public class TripsDatabaseService : IDatabaseService
	{
		private readonly DatabaseContext _dbContext;

		public TripsDatabaseService(DatabaseContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task DeleteClient(int idClient)
		{
			var client = new Client() { IdClient = idClient };

			_dbContext.Clients.Attach(client);
			_dbContext.Clients.Remove(client);

			await _dbContext.SaveChangesAsync();
		}
		public async Task<IEnumerable<TripDetailsDto>> GetTrips()
		{
			return await _dbContext.Trips
				.Select(trip => new TripDetailsDto
				{
					Name = trip.Name,
					Description = trip.Name,
					DateFrom = trip.DateFrom,
					DateTo = trip.DateTo,
					MaxPeople = trip.MaxPeople,
					Countries = trip.CountryTrips
						.Select(countryTrip => new CountryDto
						{
							Name = countryTrip.IdCountryNavigation.Name
						}).ToList(),
					Clients = trip.ClientTrips
						.Select(clientTrip => new ClientDto
						{
							FirstName = clientTrip.IdClientNavigation.FirstName,
							LastName = clientTrip.IdClientNavigation.LastName
						}).ToList()
				})
				.OrderByDescending(trip => trip.DateFrom)
				.ToListAsync();
		}

		public async Task<bool> IsClientAssigned(int idClient)
		{
			return await _dbContext.Clients
				.Where(client => client.IdClient == idClient)
				.AnyAsync(client => client.ClientTrips.Any());
		}

		
		public async Task AssignClient(AssignClientToTripDto payload)
		{
			var client = await GetClientAsync(payload.Pesel);

			var clientTrip = new ClientTrip()
			{
				IdClientNavigation = client,
				IdTrip = payload.IdTrip,
				RegisteredAt = DateTime.Now,
				PaymentDate = payload.PaymentDate
			};

			_dbContext.ClientTrips.Add(clientTrip);

			await _dbContext.SaveChangesAsync();
		}

		private async Task<Client> GetClientAsync(string pesel)
		{
			return await _dbContext.Clients
				.Where(client => client.Pesel.Equals(pesel))
				.FirstOrDefaultAsync();
		}

		public async Task<int> AddClient(Client client)
		{
			var existingClient = await GetClientAsync(client.Pesel);

			if (existingClient != null)
				return existingClient.IdClient;

			_dbContext.Clients.Add(client);

			await _dbContext.SaveChangesAsync();

			return client.IdClient;
		}

		

		public async Task<bool> TripExists(int idTrip, string name)
		{
			return await _dbContext.Trips
				.AnyAsync(trip => trip.IdTrip == idTrip && trip.Name.Equals(name));
		}
		public async Task<bool> IsClientAssignedToTrip(int idClient, int idTrip)
		{
			return await _dbContext.Clients
				.Where(client => client.IdClient == idClient)
				.AnyAsync(client => client.ClientTrips
					.Any(clientTrip => clientTrip.IdTrip == idTrip)
				);
		}

		
	}
}
