using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class SaveEmployeeRequest
    {
        public Guid Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string CurrentStatus { get; set; }
        public string EmployeeType { get; set; }
        public string EmployementType { get; set; }
        public string WorkingHours { get; set; }
        public string JobTitle { get; set; }
        public DateOnly ContractualStartDate { get; set; }
        public DateOnly ContractualEndDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateOnly Birthdate { get; set; }
        public string BirthCity { get; set; }
        public string BirthCountry { get; set; }
        public string Nationality { get; set; }
        public int? TaxId { get; set; }
        public int? SocialSecurity { get; set; }
        public int? TaxClass { get; set; }
        public string? HealthInsaurance { get; set; }
        public string? MarritalStatus { get; set; }
        public string? Religiousaffiliation { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? PersonalSheet { get; set; }
        public string? SocialSecurityFile { get; set; }
        public bool EmployeeStatus { get; set; }
        public string? AnnualLeaveEntitlement { get; set; }
        public bool Unlimited { get; set; }
        public bool Adjustedenddate { get; set; }
        public bool Terminatedby { get; set; }
        public string Employees { get; set; }
        public string Employer { get; set; }
        public string Deliverymethod { get; set; }
        public string Dateofreceipt { get; set; }


        public Guid SLGStatus { get; set; }
        public Guid Designation { get; set; }
        public string RoleId { get; set; }
        //THis name is Role NAme
        public string Name { get; set; }

        public Guid? Leader1Id { get; set; }
        public Guid? Leader2Id { get; set; }
        public Guid? DefaulLeaderId { get; set; }

        public ActionEnum Action { get; set; }
        // Navigation property for LanguageCompetenceDTOs
        public virtual ICollection<LanguageCompetenceDTO> LanguageCompetences { get; set; }
        //EmployeChildern

        public bool employeehavechildren { get; set; }
        public string child_FirstName { get; set; }
        public string FamilyName { get; set; }
        public DateOnly CHild_BirthDate { get; set; }
        public string Locationchildregistered { get; set; }
        public string socialcareinsurance { get; set; }
        public string BirthCertificate { get; set; }

        //Probation

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public DateOnly? Prob_AdjustedEndDate { get; set; }
        public string? AdjustedDocument { get; set; }
        public bool adjustedenddatecheck { get; set; }
        public bool ProbationUnlimited { get; set; }

        //End of Employment
        public DateOnly NoticePeriodStartDate { get; set; }
        public DateOnly NoticePeriodEndDate { get; set; }

        //Termination of Employment
        public bool IsTerminated { get; set; }
        public DateOnly? TerminationStartDate { get; set; }
        public DateOnly? TerminationEndDate { get; set; }
        public string? Terminationofemplyee { get; set; }
        //public ICollection<LanguageCompetenceDTO> LanguageCompetences { get; set; }
    }

}
