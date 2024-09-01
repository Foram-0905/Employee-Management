using beONHR.Entities.beONHR.Entities;
using beONHR.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.Context
{
    public class MainContext:DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        //public DbSet <EmployeeChildren> EmployeeChildrens{ get; set; }
       public DbSet <BankDetails> Bankdetails{ get; set; }
        //public DbSet <EmployeeChildren> EmployeeChildrens{ get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet <City> Cities{ get; set; }
        
        public DbSet<ContactAdress> ContactAdresses { get; set; }
        public DbSet <Contact> Contacts { get; set; }
        public DbSet <Currency> Currencies{ get; set; }
        public DbSet <Document> Documents{ get; set; }
        public DbSet<DocumentList> DocumentList { get; set; }
        public DbSet <EductionLevel> EductionLevels{ get; set; }
        public DbSet <EmployeeEduction> EmployeeEductions{ get; set; }
        public DbSet <Employment> Employments{ get; set; }
        public DbSet <IdentityCard> IdentityCards{ get; set; }
        public DbSet <JobHistory> JobHistories{ get; set; }
        public DbSet <LanguageCompetence> LanguageCompetences{ get; set; }
        public DbSet <LanguageLevel> LanguageLevels{ get; set; }
        public DbSet <LeaveCategory> LeaveCategories{ get; set; }
        public DbSet <LeaveType> LeaveTypes{ get; set; }
        public DbSet <ManageDesignation> ManageDesignations{ get; set; }
        //public DbSet <Probation> Probations{ get; set; }
        public DbSet <PublicHoliday> PublicHolidays { get; set; }
        public DbSet <SLGGroup> SLGGroups{ get; set; }
       
        //public DbSet <Worklocation> Worklocations { get; set; }
        public DbSet <WorkPermitDetail> WorkPermitDetail{ get; set; }

        //public DbSet<Salary> EmpSalary { get; set; }
        public DbSet <Consultant_Rate> ConsultantRates { get; set; }
        public DbSet<Salary> EmpSalary { get; set; }
        public DbSet<SalaryType>SalaryTypes { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<ManageAssets> ManageAssets { get; set; }

        public DbSet<Assets_Status> Assets_Status { get; set; }

        public DbSet<taxclass> TaxClass { get; set; }
        public DbSet<EmployeeYearlyLeaveBalance> EmployeeYearlyLeaveBalances { get; set; }

        public DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public DbSet<EmploymentType> EmploymentTypes { get; set; }
        public DbSet<Deliverymethod> Deliverymethods { get; set; }

        public DbSet<LeaveTypeEmployee> LeaveTypeEmployees { get; set; }

        public DbSet<EmployeenStatus> EmployeenStatuses { get; set; }

        public DbSet<TypeofEmployment> TypeofEmployments {  get; set; }
        public DbSet<Assets_Type>Assets_Type{ get; set; }

        public DbSet<Education>Educations { get; set; }

        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        //public DbSet<SalaryType> SalaryTypes { get; set; }

        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<Role> Roles { get; set; }
      
        public DbSet<Notification> Notifications { get; set; }
        
        public DbSet<TransactionType> TransactionTypes { get; set; }
      

        // Inside your UserContext class
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           //Exclude AspNetRoles from migrations
           modelBuilder.Ignore<AspNetRoles>();

           

            modelBuilder.Entity<JobHistory>()
    .HasOne(jh => jh.Employee)
    .WithMany() // Assuming there's a collection navigation property in Employee referring to JobHistories
    .HasForeignKey(jh => jh.EmployeeId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Salary>()
    .HasOne(es => es.Employee)
    .WithMany() // Assuming there's a collection navigation property in Employee referring to EmpSalaries
    .HasForeignKey(es => es.EmployeeId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
    .HasOne(c => c.Employee)
    .WithMany() // Assuming there's a collection navigation property in Employee referring to Contacts
    .HasForeignKey(c => c.EmployeeId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
    .HasOne(e => e.Country)
    .WithMany() // Assuming there's no collection navigation property in Countries referring to Employees
    .HasForeignKey(e => e.CurrentStatusId)
    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<EmployeeEduction>()
    .HasOne(ee => ee.Employee)
    .WithMany() 
    .HasForeignKey(ee => ee.EmployeeId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Education>()
        .HasOne(ed => ed.Employees)
    .WithMany() // Assuming there's a collection navigation property in Employee referring to Educations
    .HasForeignKey(ed => ed.Employee)
    .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<City>()
              .HasIndex(c => c.Name);

            modelBuilder.Entity<State>()
                .HasIndex(c => c.Name);
               
            modelBuilder.Entity<City>()
               .HasOne(c => c.stateId)
               .WithMany()
               .HasForeignKey(c => c.State)
               .OnDelete(DeleteBehavior.Restrict); // Specify Restrict instead of NoAction

            // Configure the foreign key relationship
            modelBuilder.Entity<PublicHoliday>()
                .HasOne(p => p.stateId)
                .WithMany()
                .HasForeignKey(p => p.State)
                .OnDelete(DeleteBehavior.NoAction); // Specify ON DELETE NO ACTION

            // Configure the foreign key relationship between Employees and SLGGroups
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.SLGGroup)
                .WithMany()
                .HasForeignKey(e => e.SLGStatus)
                .OnDelete(DeleteBehavior.NoAction); // Specify ON DELETE NO ACTION

            // Configure the foreign key relationship between ContactAdresses and Countries
           

            modelBuilder.Entity<EmployeeEduction>()
                .HasOne(e => e.countryId)
                .WithMany()
                .HasForeignKey(e => e.Country)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<JobHistory>()
                .HasOne(j => j.countryId)
                .WithMany()
                .HasForeignKey(j => j.Country)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ManageAssets>()
            .HasOne(m => m.PreviousOwnerEmployee)
            .WithMany()
            .HasForeignKey(m => m.PreviousOwner)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ManageAssets>()
            .HasOne(m => m.PreviousOwnerEmployee)
            .WithMany()
            .HasForeignKey(m => m.PreviousOwner)
            .OnDelete(DeleteBehavior.NoAction);

            // Configure the foreign key relationship between Worklocations and States
           

            modelBuilder.Entity<Employee>()
         .HasOne(e => e.Leader1)
         .WithMany()
         .HasForeignKey(e => e.Leader1Id)
         .OnDelete(DeleteBehavior.Restrict); // No action for Leader1

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Leader2)
                .WithMany()
                .HasForeignKey(e => e.Leader2Id)
                .OnDelete(DeleteBehavior.Restrict); // No action for Leader2

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.DefaulLeader3)
                .WithMany()
                .HasForeignKey(e => e.DefaulLeaderId)
                .OnDelete(DeleteBehavior.NoAction); // NoAction delete for DefaultLeader

            modelBuilder.Entity<JobHistory>()
             .HasOne(e => e.State)
             .WithMany()
             .HasForeignKey(e => e.StateId)
             .OnDelete(DeleteBehavior.NoAction); // NoAction delete for DefaultLeader

            modelBuilder.Entity<Education>()
       .HasOne(e => e.countryid)
       .WithMany()
       .HasForeignKey(e => e.Country)
       .OnDelete(DeleteBehavior.NoAction); // No NoAction delete for Country

            

            modelBuilder.Entity<Education>()
       .HasOne(e => e.stateid)
       .WithMany()
       .HasForeignKey(e => e.State)
       .OnDelete(DeleteBehavior.NoAction); // No NoAction delete for State

            modelBuilder.Entity<JobHistory>()
       .HasOne(j => j.State)
       .WithMany()
       .HasForeignKey(j => j.StateId)
       .OnDelete(DeleteBehavior.NoAction); // No NoAction delete for State


            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

          

            modelBuilder.Entity<Contact>()
            .HasOne(c => c.WorkCountry)                 // Assuming WorkCountry is the navigation property representing the country where the contact works
            .WithMany()                                 // Assuming a country can have many contacts
            .HasForeignKey(c => c.WorkCountryId)       // Assuming WorkCountryId is the foreign key property in the Contacts entity
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Contact>()
            .HasOne(c => c.WorkState)                   // Assuming WorkState is the navigation property representing the state where the contact works
            .WithMany()                                 // Assuming a state can have many contacts
            .HasForeignKey(c => c.WorkStateId)         // Assuming WorkStateId is the foreign key property in the Contacts entity
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ContactAdress>()
            .HasOne(ca => ca.ContactCountry)                // Assuming ContactCountry is the navigation property representing the country of the contact address
            .WithMany()                                     // Assuming a country can have many contact addresses
            .HasForeignKey(ca => ca.ContactCountryId)       // Assuming ContactCountryId is the foreign key property in the ContactAdresses entity
            .OnDelete(DeleteBehavior.Restrict);             // Specifies that no action is taken on delete

            modelBuilder.Entity<ContactAdress>()
            .HasOne(ca => ca.ContactState)                  // Assuming ContactState is the navigation property representing the state of the contact address
            .WithMany()                                     // Assuming a state can have many contact addresses
            .HasForeignKey(ca => ca.ContactStateId)         // Assuming ContactStateId is the foreign key property in the ContactAdresses entity
            .OnDelete(DeleteBehavior.Restrict);             // Specifies that no action is taken on delete


        }

    }
}
