using beONHR.Entities.DTO.Enum;
using beONHR.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class EmployeeDTO
    {
        //*********General**********//
        public Guid Id { get; set; } //check
        public int EmployeeNumber { get; set; }
        public Guid CurrentStatusId { get; set; }
        public Guid EmployementTypeId { get; set; }

        public string? EmployementTypename { get; set; }

        public Guid TypeofEmploymentId { get; set; }
        public string WorkingHours { get; set; }
        public string JobTitle { get; set; }
        public DateOnly ContractualStartDate { get; set; }
        public DateOnly ContractualEndDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }
        public DateOnly Birthdate { get; set; }
        public string BirthCity { get; set; }
        public Guid BirthCountryId { get; set; }
        public string Nationality { get; set; }
        public string? TaxId { get; set; }
        public string? SocialSecurity { get; set; }
        public Guid? TaxClassId { get; set; }
        public string? HealthInsaurance { get; set; }
        public Guid? MaritalStatusId { get; set; }
        public string? Religiousaffiliation { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? PersonalSheet { get; set; }
        public string? SocialSecurityFile { get; set; }
        public Guid EmployeeStatusId { get; set; }

        public string? EmployeeStatusname { get; set; }





        //*********Children information**********//

        public bool employeehavechildren { get; set; }
        public string? child_FirstName { get; set; }
        public string? FamilyName { get; set; }
        public DateOnly? CHild_BirthDate { get; set; }
        public string? Locationchildregistered { get; set; }
        public string? socialcareinsurance { get; set; }
        public string? BirthCertificate { get; set; }


        //*********Language Competence**********//

        public virtual ICollection<LanguageCompetenceDTO> LanguageCompetences { get; set; }

        //*********LeaveType**********//
        public virtual ICollection<EmployeeYearlyLeaveBalanceDTO> EmployeeYearlyLeaveBalances { get; set; }



        //*********Job Title and Role**********//
        public string RoleId { get; set; }

        public string Rolename { get; set; }

        public Guid SLGStatus { get; set; }

        public string SLGStatusname { get; set; }

        public Guid Designation { get; set; }



        //*********Leadership**********//

        public Guid? Leader1Id { get; set; }
        public Guid? Leader2Id { get; set; }
        public Guid? DefaulLeaderId { get; set; }



        //*********Probation**********//
        public bool Istheemployeeonprobation { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? Prob_AdjustedEndDate { get; set; }
        public string? AdjustedDocument { get; set; }
        public bool? adjustedenddatecheck { get; set; }
        public bool? ProbationUnlimited { get; set; }




        //*********End of Employment**********//
        public bool Isthistheendofemployment { get; set; }
        public DateOnly? NoticePeriodStartDate { get; set; }
        public DateOnly? NoticePeriodEndDate { get; set; }



        //*********Termination of Employment**********//

        public bool IsTerminated { get; set; }
        public DateOnly? TerminationStartDate { get; set; }
        public DateOnly? TerminationEndDate { get; set; }
        public string? Terminationofemplyee { get; set; }
        public DateOnly? Dateofreceipt { get; set; }
        public Guid? DeliverymethodId { get; set; }

        public ActionEnum Action { get; set; }



        ///********Contact***********////
        public string OfficeEmail { get; set; }
        public int? WorkLocation { get; set; }
        public string? PhoneNumber { get; set; }


    }
    public class ResponseEmployeeDTO
    {
        public List<EmployeeDTO> Employee { get; set; }
        public int TotalRecord { get; set; }


    }
    public class LanguageCompetenceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Level { get; set; }
        public string LevelName { get; set; }
        public Guid EmployeeId { get; set; }
        public string LanguagesCertificate { get; set; }

    }
}
