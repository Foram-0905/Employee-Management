using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beONHR.Entities
{
    public class ManageAssets
    {
        [Key]
        public Guid Id { get; set; }

        
        public string SerialNumber { get; set; }

        
        public Guid Status { get; set; }

        [ForeignKey("Status")]
        public virtual Assets_Status Statusname { get; set; }


        public string Manufacturer { get; set; }

        
        public Guid AssetType { get; set; }

        [ForeignKey("AssetType")]
        public virtual Assets_Type AssetTypename { get; set; }


        public string Model { get; set; }

        public string MoreDetails { get; set; }
 
        public DateOnly PurchaseDate { get; set; }

        public DateOnly WarrantyDueDate { get; set; }

        // Store warranty file in Base64 format
        public string? Warranty { get; set; }

        
        public Nullable<Guid> CurrentOwner { get; set; }

        [ForeignKey("CurrentOwner")]
        public virtual Employee? CurrentOwnerEmployee { get; set; }

        
        public Nullable<Guid> PreviousOwner { get; set; }

        [ForeignKey("PreviousOwner")]
        public virtual Employee? PreviousOwnerEmployee { get; set; }

        public string Note { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}   
