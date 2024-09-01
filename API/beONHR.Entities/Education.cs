using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace beONHR.Entities
{
    public class Education
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EducationLevels { get; set; }

        [ForeignKey("EducationLevels")]
        public virtual EductionLevel EducationLevelid  { get; set; }


        public string Subject { get; set; }

        public string Institute { get; set; }


        public Guid City {  get; set; }

        [ForeignKey("City")]
        [JsonIgnore]
        public virtual City cityid { get; set; }

        public Guid State { get; set; }

        [ForeignKey("State")]
        [JsonIgnore]
        public virtual State stateid { get; set; }


        public Guid Country { get; set; }

        [ForeignKey("Country")]
        [JsonIgnore]
        public virtual Country countryid { get; set; }


        public DateOnly CompletionDate { get; set; }


        // Store warranty file in Base64 format
        public string Certificate { get; set; }
            

        public string? Anabin { get; set; }

        public Guid Employee { get; set; }

        [ForeignKey("Employee")]
        public virtual Employee Employees { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
