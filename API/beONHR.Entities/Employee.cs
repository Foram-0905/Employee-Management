    using beONHR.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities
{
    public class Employee
    {
        //*********General**********//
        [Key]
        public Guid Id { get; set; } //check
        public int EmployeeNumber { get; set; }
        public Guid CurrentStatusId { get; set; }

        [ForeignKey("CurrentStatusId")]
        public virtual Country? Country { get; set; }
        public Guid EmployementTypeId { get; set; }

        [ForeignKey("EmployementTypeId")]
        public virtual EmploymentType? EmploymentType { get; set; }
        public Guid TypeofEmploymentId { get; set; }

        [ForeignKey("TypeofEmploymentId")]
        public virtual TypeofEmployment TypeofEmployment { get; set; }
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
        public Guid BirthCountryId { get; set;  }
        [ForeignKey("BirthCountryId")]
        public virtual Country? BirthCountry { get; set; }
        public string Nationality { get; set; }
        public string? TaxId { get; set; }
        public string? SocialSecurity { get; set; }
        public Guid? TaxClassId { get; set; }

        [ForeignKey("TaxClassId")]
        public virtual taxclass? taxclass { get; set; }
        public string? HealthInsaurance { get; set; }
        public Guid? MaritalStatusId { get; set; }

        [ForeignKey("MaritalStatusId")]
        public virtual MaritalStatus? MaritalStatus { get; set; }
        public string? Religiousaffiliation { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? PersonalSheet { get; set; }
        public string? SocialSecurityFile { get; set; }
        public Guid EmployeeStatusId { get; set; }

        [ForeignKey("EmployeeStatusId")]
        public virtual EmployeenStatus? EmployeenStatus { get; set; }
      

        //*********Children information**********//

        public bool employeehavechildren { get; set; }
        public string? child_FirstName { get; set; }
        public string? FamilyName { get; set; }
        public DateOnly? CHild_BirthDate { get; set; }
        public string? Locationchildregistered { get; set; }
        public string? socialcareinsurance { get; set; }
        public string? BirthCertificate { get; set; }


        //*********Language Competence**********//

        public virtual ICollection<LanguageCompetence> LanguageCompetences { get; set; }

        //*********LeaveType**********//
        public virtual ICollection<EmployeeYearlyLeaveBalance> EmployeeYearlyLeaveBalances { get; set; }

        //*********Job Title and Role**********//
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual AspNetRoles Role { get; set; }
        public Guid SLGStatus { get; set; }

        [ForeignKey("SLGStatus")]
        public virtual SLGGroup? SLGGroup { get; set; }
        public Guid Designation { get; set; }

        [ForeignKey("Designation")]
        public virtual ManageDesignation? ManageDesignation { get; set; }


        //*********Leadership**********//

        public Guid? Leader1Id { get; set; }
        public Guid? Leader2Id { get; set; }
        public Guid? DefaulLeaderId { get; set; }       

        [ForeignKey("Leader1Id")]
        public virtual Employee Leader1 { get; set; }

        [ForeignKey("Leader2Id")]
        public virtual Employee Leader2 { get; set; }

        [ForeignKey("DefaulLeaderId")]
        public virtual Employee DefaulLeader3 { get; set; }


        //*********Probation**********//
        public bool Istheemployeeonprobation {  get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? Prob_AdjustedEndDate { get; set; }
        public string? AdjustedDocument { get; set; }
        public bool? adjustedenddatecheck { get; set; }
        public bool? ProbationUnlimited { get; set; }

        
        //*********End of Employment**********//
        public bool Isthistheendofemployment {  get; set; }
        public DateOnly? NoticePeriodStartDate { get; set; }
        public DateOnly? NoticePeriodEndDate { get; set; }



        //*********Termination of Employment**********//
    
        public bool IsTerminated { get; set; }
        public DateOnly? TerminationStartDate { get; set; }
        public DateOnly? TerminationEndDate { get; set; }
        public string? Terminationofemplyee { get; set; }
        public DateOnly? Dateofreceipt { get; set; }
        public Guid? DeliverymethodId {  get; set; }

        [ForeignKey("DeliverymethodId")]
        public virtual Deliverymethod? Deliverymethod { get; set; }


        public virtual ICollection<DocumentList> DocumentList { get; set; }



        public bool IsDeleted { get; set; } = false;

    }
}


//*********Language Competence**********//


namespace beONHR.Entities
{
    public class LanguageCompetence
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid Level { get; set; } 
        public string? LanguagesCertificate { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("Level")]
        public virtual LanguageLevel? LanguageLevel { get; set; }
        public Guid EmployeeId { get; set; }
   
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}
