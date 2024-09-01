using beONHR.Entities.DTO.Enum;
using System;

namespace beONHR.Entities.DTO
{
    public class Assets_typeDTO
    {
        public Guid Id { get; set; }
        public string AssetsTypes { get; set; }

        public ActionEnum Action { get; set; }
    }
    public class ResponseAssets_typeDto
    {
        public List<ManageAssets> Assets_type { get; set; }
        public int TotalRecord { get; set; }


    }
}
