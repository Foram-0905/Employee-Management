using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class TypeofEmploymentDTO
    {
        public Guid Id { get; set; }
        public string typeofemployment { get; set; }

    }
    public class ResponseTypeofEmploymentDTO
    {
        public List<ManageAssets> TypeofEmployment { get; set; }
        public int TotalRecord { get; set; }


    }
}
