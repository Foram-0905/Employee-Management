using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class TransactionTypeDTO
    {
        public Guid Id { get; set; }
        public bool Transaction { get; set; }
        public CreditDebit creditDebit { get; set; }

    }
}
