using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using trips.Models;
using trips.Models.DTO;

namespace trips.Services
{
	public interface IDatabaseService
	{
		Task<IEnumerable<TripDetailsDto>> GetTrips();
		Task<bool> IsClientAssigned(int idClient);
		Task DeleteClient(int idClient);
		Task<int> AddClient(Client client);
		Task<bool> IsClientAssignedToTrip(int idClient, int idTrip);
		Task<bool> TripExists(int idTrip, string name);
		Task AssignClient(AssignClientToTripDto client);
	}
}
