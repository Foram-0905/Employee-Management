using beONHR.Entities.DTO.beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class ContactDTO
    {
        public Guid Id { get; set; }
        public int WorkZipCode { get; set; }
        public Guid WorkCity { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid WorkStateId { get; set; }
        public Guid WorkCountryId { get; set; }
        public ActionEnum Action { get; set; }
        public virtual ICollection<ContactAdressDTO> ContactAdressDetails { get; set; }
        public virtual ICollection<BankDetailsDTO> BankDetails { get; set; }
    }

    namespace beONHR.Entities.DTO
    {
        public class ContactAdressDTO

        {
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
            public ActionEnum Action { get; set; }

        }

    }
    namespace beONHR.Entities.DTO
    {
        public class BankDetailsDTO

        {
            public Guid Id { get; set; }
            public string BankAccountNumber { get; set; }
            public string BankIFSCCode { get; set; }
            public string BankName { get; set; }
            public string BankAccountHolder { get; set; }
            public ActionEnum Action { get; set; }

        }
    }
}
