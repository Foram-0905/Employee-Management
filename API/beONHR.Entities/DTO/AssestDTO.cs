using beONHR.Entities.DTO.Enum;
using System;

namespace beONHR.Entities.DTO
{
    public class AssetsDTO
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public Guid Status { get; set; }
        public string StatusName { get; set; }
        public string Manufacturer { get; set; }
        public Guid AssetType { get; set; }
        public string AssetTypeName { get; set; }
        public string Model { get; set; }
        public string MoreDetails { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public DateOnly WarrantyDueDate { get; set; }
        public string? Warranty { get; set; }
        public Guid? CurrentOwner { get; set; }
        public string CurrentOwnerFullName { get; set; }
        //public string CurrentOwnerMiddleName { get; set; }
        //public string CurrentOwnerLastName { get; set; }
        public Guid? PreviousOwner { get; set; }
        public string PreviousOwnerFullName { get; set; }
        //public string PreviousOwnerMiddleName { get; set; }
        //public string PreviousOwnerLastName { get; set; }
        public string Note { get; set; }

        public ActionEnum Action { get; set; }
    }
    public class ResponseAssetsDto
    {
        public List<AssetsDTO> Assets { get; set; }
        public int TotalRecord { get; set; }


    }
}
