using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class TransactionType
    {
        public Guid Id { get; set; }
        public bool Transaction {  get; set; }
        public string TransactionName { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
