namespace Example.Models
{
	using System;
	using System.Collections.Generic;
	using Entities;
	using Entities.Enums;
	using Selp.Interfaces;

	public class PolicyModel : ISelpEntity<int>
	{
		public DateTime CreatedDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public DateTime StartDate { get; set; }

		public decimal InsurancePremium { get; set; }
		public decimal InsuranceSum { get; set; }

		public PolicyStatus Status { get; set; }

		public string UserId { get; set; }

		public List<int> InsuredList { get; set; } //insured

		public int RegionId { get; set; }

		public List<Guid> Attachments { get; set; }

		public int Id { get; set; }
	}
}