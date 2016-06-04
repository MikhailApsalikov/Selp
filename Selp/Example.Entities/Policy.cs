namespace Example.Entities
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;
	using Enums;
	using Selp.Interfaces;

	public class Policy : ISelpEntity<int>
	{
	    public string Serial { get; set; }
	    public string Number { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public DateTime StartDate { get; set; }

		public decimal InsurancePremium { get; set; }
		public decimal InsuranceSum { get; set; }

		public PolicyStatus Status { get; set; }

		public string UserId { get; set; }
		public User User { get; set; } // user creator

		[InverseProperty("Policies")]
		public ICollection<Party> Parties { get; set; } //insured

		public int RegionId { get; set; }

		public virtual Region Region { get; set; }

		public int Id { get; set; }
	}
}