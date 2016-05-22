namespace Example.Models
{
	using Selp.Interfaces;

	public class PolicyShortModel : ISelpEntity<int>
	{
		public string Serial { get; set; }
		public string Number { get; set; }

		public string Status { get; set; }
		public string UserId { get; set; }
		public decimal InsurancePremium { get; set; }
		public decimal InsuranceSum { get; set; }

		public string CreatedDate { get; set; }
		public string ExpirationDate { get; set; }
		public string StartDate { get; set; }

		public int Id { get; set; }
	}
}