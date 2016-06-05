namespace Example.Models
{
    using System.Collections.Generic;
    using Selp.Interfaces;

    public class PolicyModel : ISelpEntity<int>
    {
        public string Serial { get; set; }
        public string Number { get; set; }

        public string CreatedDate { get; set; }
        public string ExpirationDate { get; set; }
        public string StartDate { get; set; }

        public decimal InsurancePremium { get; set; }
        public decimal InsuranceSum { get; set; }

        public string Status { get; set; }

        public string UserId { get; set; }
        public List<int> Insured { get; set; }
        public int RegionId { get; set; }

        public string Region { get; set; }
        public int Id { get; set; }
    }
}