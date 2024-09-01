using beONHR.Entities.DTO.Enum;
using System.Text.Json.Serialization;

namespace beONHR.Entities.DTO
{
    public class IdentityCardDTO
    {
        public Guid Id { get; set; }
        public string Passport { get; set; }
        [JsonPropertyName("visaStartDate")]
        public DateOnly VisaStartdate { get; set; }
        [JsonPropertyName("visaExpiryDate")]
        public DateOnly VisaExpirytdate { get; set; }
        public string Visa { get; set; }
        [JsonPropertyName("blueCardStartDate")]
        public DateOnly BlueCardStartdate { get; set; }
        [JsonPropertyName("blueCardExpiryDate")]
        public DateOnly BlueCardExpirytdate { get; set; }
        public string BlueCard { get; set; }
        public Guid EmployeeId { get; set; }
        public ActionEnum Action { get; set; }
        public virtual ICollection<WorkPermitDetailDTO> WorkPermitDetails { get; set; }

    }
}

namespace beONHR.Entities.DTO
{
    public class WorkPermitDetailDTO
    {
        public Guid Id { get; set; }
        public string PermitType { get; set; }
        [JsonPropertyName("permitStartdate")]
        public DateOnly PermitStartdate { get; set; }
        [JsonPropertyName("permitExpirytdate")]
        public DateOnly PermitExpirytdate { get; set; }
        public string Document { get; set; }
        public ActionEnum Action { get; set; }
    }
}