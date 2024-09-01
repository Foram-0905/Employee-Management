using beONHR.Entities.DTO.Enum;
using System;
using System.Text.Json.Serialization;

namespace beONHR.Entities.DTO
{
    public class EducationDTO
    {
        public Guid Id { get; set; }
        public Guid EducationLevels { get; set; }
        public string EducationLevelName { get; set; }
        public string Subject { get; set; }

        public string Institute { get; set; }

        public Guid City { get; set; }

        public Guid State { get; set; }

        public Guid Country { get; set; }

        public DateOnly CompletionDate { get; set; }

        // Store warranty file in Base64 format
        public string Certificate { get; set; }

        public string? Anabin { get; set; }

        public Guid Employee { get; set; }

        public ActionEnum Action { get; set; }
        public string Filename { get; set; }
        //public Guid EmployeeId { get; set; } 
    }
    public class ResponseEducationDto
    {
        public List<EducationDTO> Education { get; set; }
        public int TotalRecord { get; set; }


    }
}