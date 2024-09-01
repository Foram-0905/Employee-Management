using beONHR.Entities.beONHR.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beONHR.Entities
{
    public class Contact
    {
        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<ContactAdress> ContactAdressDetails { get; set; }

        // Work Location properties
        public int WorkZipCode { get; set; }
        public Guid WorkCity { get; set; }
        public Guid WorkStateId { get; set; }
        public Guid WorkCountryId { get; set; }

        // Bank Details properties
        public virtual ICollection<BankDetails> Bankdetails { get; set; }

        public Guid EmployeeId { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        [ForeignKey("WorkCity")]
        public virtual City? WorkCityObj { get; set; }

        [ForeignKey("WorkStateId")]
        public virtual State? WorkState { get; set; }

        [ForeignKey("WorkCountryId")]
        public virtual Country? WorkCountry { get; set; }
    }
    namespace beONHR.Entities
    {
        public class ContactAdress
        {
            [Key]
            public Guid Id { get; set; }
            public string Number { get; set; }
            public string Street { get; set; }
            public Guid ContactStateId { get; set; }
            public Guid ContactCountryId { get; set; }
            public int ContactZipCode { get; set; }
            public Guid ContactCity { get; set; }
            public string ContactPhone1 { get; set; }
            public string? ContactPhone2 { get; set; }
            public string ContactEmailbeON { get; set; }
            public string ContactEmailPrivate { get; set; }
            public bool ContactEntitlement { get; set; }

            [ForeignKey("ContactCity")]
            public virtual City? ContactCityObj { get; set; }

            [ForeignKey("ContactStateId")]
            public virtual State? ContactState { get; set; }

            [ForeignKey("ContactCountryId")]
            public virtual Country? ContactCountry { get; set; }

            public Guid ContactId { get; set; }

            [ForeignKey("ContactId")]
            public virtual Contact? Contact { get; set; }
        }
    }
    namespace beONHR.Entities
    {
        public class BankDetails
        {
            [Key]
            public Guid Id { get; set; }
            public string BankAccountNumber { get; set; }
            public string BankIFSCCode { get; set; }
            public string BankName { get; set; }
            public string BankAccountHolder { get; set; }

            public Guid ContactId { get; set; }

            [ForeignKey("ContactId")]
            public virtual Contact? Contact { get; set; }
        }
    }

}