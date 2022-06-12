using System;
using System.Collections.Generic;

namespace trips.Models.DTO
{
	public class TripDetailsDto
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		public int MaxPeople { get; set; }
		public IEnumerable<CountryDto> Countries { get; set; }
		public IEnumerable<ClientDto> Clients { get; set; }
	}
}
