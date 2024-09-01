using beONHR.Entities.DTO.Enum;
using System;

namespace beONHR.Entities.DTO
{
    public class Assets_StatusDTO
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
        
        public ActionEnum Action { get; set; }
    }
    public class ResponseAssets_StatusDto
    {
        public List<ManageAssets> Assets_status { get; set; }
        public int TotalRecord { get; set; }


    }
}
