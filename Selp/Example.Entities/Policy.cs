namespace Example.Entities
{
	using System;
	using System.Collections.Generic;
	using Selp.Interfaces;

	public class Policy : ISelpEntity<int>
	{
		public DateTime CreatedDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public DateTime StartDate { get; set; }

		public decimal InsurancePremium { get; set; }
		public decimal InsuranceSum { get; set; }

		public string UserId { get; set; }
		public User User { get; set; } // user creator

		public ICollection<Party> Parties { get; set; } //insured

		public int RegionId { get; set; }

		public virtual Region Region { get; set; }

		public ICollection<Party> Attachments { get; set; }

		public int Id { get; set; }
	}
}