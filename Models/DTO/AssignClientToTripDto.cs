using System;
using System.ComponentModel.DataAnnotations;

namespace trips.Models.DTO
{
	public class AssignClientToTripDto
	{
		[Required]
		[StringLength(120, MinimumLength = 3)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(120, MinimumLength = 3)]
		public string LastName { get; set; }
		
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[Phone]
		public string Telephone { get; set; }

		[Required]
		[RegularExpression("\\d{11}")]
		public string Pesel { get; set; }

		[Required]
		public int IdTrip { get; set; }

		[Required]
		public string TripName { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? PaymentDate { get; set; }
	}
}
