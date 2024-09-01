using beONHR.DAL;
using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.User;
using beONHR.Infrastructure;
using beONHR.Infrastructure.Service;
using beONHR.Infrastructure.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace beONHR.API.ServiceExtensions
{
    public static class ServiceExtensions
    {


        public static void InstallConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("SMTPConfig");
            services.Configure<EmailConfiguration>(appSettingsSection);
        }
        public static void ConfigureDIServices(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
       options.JsonSerializerOptions.PropertyNameCaseInsensitive = false);


            services.AddDbContext<UserContext>(
            options => options.UseSqlServer("name=Connection"));

            services.AddDbContext<MainContext>(
            options => options.UseSqlServer("name=Connection"));


            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepo, UserRepo>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IEmailRepo, EmailRepo>();

            services.AddTransient<IDesignationService, DesignationService>();
            services.AddTransient<IDesignationRepo, DesignationRepo>();

            services.AddTransient<ISlggroupService, SlggroupService>();
            services.AddTransient<ISlggroupRepo, SlggroupRepo>();

            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<ICountryRepo, CountryRepo>();

            services.AddTransient<ICurrencyService, CurrencyService>();
            services.AddTransient<ICurrencyRepo, CurrencyRepo>();

            services.AddTransient<ICityService, CityService>();
            services.AddTransient<ICityRepo, CityRepo>();

            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IStateRepo, StateRepo>();

            services.AddTransient<IPublicHolidayService, PublicHolidayService>();
            services.AddTransient<IPublicHolidayRepo, PublicHolidayRepo>();

            services.AddTransient<ILanguageLevelService, LanguageLevelService>();
            services.AddTransient<ILanguageLevelRepo, LanguageLevelRepo>();

            services.AddTransient<ILeaveCategoryService, LeavecategoryService>();
            services.AddTransient<ILeaveCategoryRepo, LeaveCategoryRepo>();

            services.AddTransient<IEductionLevelService, EductionLevelService>();
            services.AddTransient<IEductionLevelRepo, EductionLevelRepo>();

            //services.AddTransient<ILanguageCompetenceService, LanguageCompetenceService>();
            //services.AddTransient<ILanguageCompetenceRepo, LanguageCompetenceRepo>();

            services.AddTransient<ILeaveTypeService, LeaveTypeService>();
            services.AddTransient<ILeaveTypeRepo, LeaveTypeRepo>();

            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IRoleRepo, RoleRepo>();

            services.AddTransient<ILeaveCategoryService, LeavecategoryService>();
            services.AddTransient<ILeaveCategoryRepo, LeaveCategoryRepo>();


            services.AddTransient<IEmployeeRepo, EmployeeRepo>();
            services.AddTransient<IEmployeeService, EmployeeService>();

            services.AddTransient<IAssetsService, AssetsService>();
            services.AddTransient<IAssetsRepo, AssetsRepo>();

            services.AddTransient<IPermissionService,PermissionService>();
            services.AddTransient<IPermissionRepo,PermissionRepo>();

            services.AddTransient<IAssets_StatusService, Assets_StatusService>();
            services.AddTransient<IAssets_StatusRepo, Assets_StatusRepo>();

            services.AddTransient<IAssets_typeService, Assets_typeService>();
            services.AddTransient<IAssets_typeRepo, Assets_typeRepo>();



            //services.AddTransient<IPermissionService, PermissionService>();
            //services.AddTransient<IPermissionRepo, PermissionRepo>(); 
            
            services.AddTransient<IMangeLeaveService, MangeLeaveService>();
            services.AddTransient<IMangeLeaveRepo, MangeLeaveRepo>();     
            
            services.AddTransient<ILeaveService, LeaveService>();
            services.AddTransient<ILeaveRepo, LeaveRepo>();

            services.AddTransient<IBonusService, BonusService>();
            services.AddTransient<IBonusRepo, BonusRepo>();

            services.AddTransient<ISalaryTypeService, SalaryTypeService>();
            services.AddTransient<ISalaryTypeRepo, SalaryTypeRepo>();


            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDocumentRepo, DocumentRepo>();

            services.AddTransient <ISalaryService, SalaryService > ();
            services.AddTransient<ISalaryRepo, SalaryRepo>();

            services.AddTransient<ISalaryTypeService, SalaryTypeService>();
            services.AddTransient<ISalaryTypeRepo, SalaryTypeRepo>();

            services.AddTransient<ITransactionService, TransactionTypeService>();
            services.AddTransient<ITransactionRepo, TransactionTypeRepo>();  
            
            services.AddTransient<IOrganisationalchartService, OrganisationalchartService>();
            services.AddTransient<IOrganisationalchartRepo, OrganisationalchartRepo>();

            services.AddTransient<IIdentityService, IdentityCardService>();
            services.AddTransient<IIdentityRepo, IdentityRepo>();

            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationRepo, NotificationRepo>();

            services.AddTransient<IConsultantRateService, ConsultantRateService>();
            services.AddTransient<IConsultantRateRepo, ConsultantRateRepo>();


            services.AddAuthorization(options =>
            {
                options.AddPolicy("configuration.designation.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.designation.Add")); });
                options.AddPolicy("configuration.designation.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.designation.Edit")); });
                options.AddPolicy("configuration.designation.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.designation.View")); });
                options.AddPolicy("configuration.designation.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.designation.Delete")); });
                options.AddPolicy("configuration.slggroup.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.slggroup.Add")); });
                options.AddPolicy("configuration.slggroup.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.slggroup.Edit")); });
                options.AddPolicy("configuration.slggroup.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.slggroup.View")); });
                options.AddPolicy("configuration.slggroup.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.slggroup.Delete")); });
                options.AddPolicy("configuration.role.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.role.Add")); });
                options.AddPolicy("configuration.role.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.role.Edit")); });
                options.AddPolicy("configuration.role.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.role.View")); });
                options.AddPolicy("configuration.role.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.role.Delete")); });
                options.AddPolicy("configuration.permission.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.permission.Add")); });
                options.AddPolicy("configuration.permission.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.permission.Edit")); });
                options.AddPolicy("configuration.permission.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.permission.View")); });
                options.AddPolicy("configuration.permission.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.permission.Delete")); });
                options.AddPolicy("configuration.leave.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leave.Add")); });
                options.AddPolicy("configuration.manageleave.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageleave.Edit")); });
                options.AddPolicy("configuration.manageleave.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageleave.View")); });
                options.AddPolicy("configuration.manageleave.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageleave.Delete")); });
                options.AddPolicy("configuration.leavecategory.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavecategory.Add")); });
                options.AddPolicy("configuration.leavecategory.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavecategory.Edit")); });
                options.AddPolicy("configuration.leavecategory.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavecategory.View")); });
                options.AddPolicy("configuration.leavecategory.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavecategory.Delete")); });
                options.AddPolicy("configuration.leavetype.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavetype.Add")); });
                options.AddPolicy("configuration.leavetype.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavetype.Edit")); });
                options.AddPolicy("configuration.leavetype.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavetype.View")); });
                options.AddPolicy("configuration.leavetype.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.leavetype.Delete")); });
                options.AddPolicy("configuration.managepublicholidays.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managepublicholidays.Add")); });
                options.AddPolicy("configuration.managepublicholidays.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managepublicholidays.Edit")); });
                options.AddPolicy("configuration.managepublicholidays.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managepublicholidays.View")); });
                options.AddPolicy("configuration.managepublicholidays.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managepublicholidays.Delete")); });
                options.AddPolicy("configuration.manageasset.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageasset.Add")); });
                options.AddPolicy("configuration.manageasset.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageasset.Edit")); });
                options.AddPolicy("configuration.manageasset.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageasset.View")); });
                options.AddPolicy("configuration.manageasset.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.manageasset.Delete")); });
                options.AddPolicy("configuration.educationlevel.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.educationlevel.Add")); });
                options.AddPolicy("configuration.educationlevel.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.educationlevel.Edit")); });
                options.AddPolicy("configuration.educationlevel.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.educationlevel.View")); });
                options.AddPolicy("configuration.educationlevel.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.educationlevel.Delete")); });
                options.AddPolicy("configuration.languagelevel.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.languagelevel.Add")); });
                options.AddPolicy("configuration.languagelevel.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.languagelevel.Edit")); });
                options.AddPolicy("configuration.languagelevel.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.languagelevel.Delete")); });
                options.AddPolicy("configuration.languagelevel.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.languagelevel.View")); });
                options.AddPolicy("configuration.managecurrency.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecurrency.Add")); });
                options.AddPolicy("configuration.managecurrency.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecurrency.Edit")); });
                options.AddPolicy("configuration.managecurrency.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecurrency.Delete")); });
                options.AddPolicy("configuration.managecurrency.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecurrency.View")); });
                options.AddPolicy("configuration.managecountry.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecountry.Add")); });
                options.AddPolicy("configuration.managecountry.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecountry.Edit")); });
                options.AddPolicy("configuration.managecountry.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecountry.Delete")); });
                options.AddPolicy("configuration.managecountry.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecountry.View")); });
                options.AddPolicy("configuration.stateregion.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.stateregion.Add")); });
                options.AddPolicy("configuration.stateregion.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.stateregion.Edit")); });
                options.AddPolicy("configuration.stateregion.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.stateregion.Delete")); });
                options.AddPolicy("configuration.stateregion.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.stateregion.View")); });
                options.AddPolicy("configuration.managecity.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecity.Add")); });
                options.AddPolicy("configuration.managecity.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecity.Edit")); });
                options.AddPolicy("configuration.managecity.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecity.Delete")); });
                options.AddPolicy("configuration.managecity.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.managecity.View")); });
                options.AddPolicy("configuration.useractivelog.Add", builder => { builder.AddRequirements(new PermissionRequirement("configuration.useractivelog.Add")); });
                options.AddPolicy("configuration.useractivelog.Edit", builder => { builder.AddRequirements(new PermissionRequirement("configuration.useractivelog.Edit")); });
                options.AddPolicy("configuration.useractivelog.Delete", builder => { builder.AddRequirements(new PermissionRequirement("configuration.useractivelog.Delete")); });
                options.AddPolicy("configuration.useractivelog.View", builder => { builder.AddRequirements(new PermissionRequirement("configuration.useractivelog.View")); });
                options.AddPolicy("employeelist.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeelist.Add")); });
                options.AddPolicy("employeelist.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeelist.Edit")); });
                options.AddPolicy("employeelist.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeelist.Delete")); });
                options.AddPolicy("employeelist.View", builder => { builder.AddRequirements(new PermissionRequirement("employeelist.View")); });
                options.AddPolicy("organisationalchart.Add", builder => { builder.AddRequirements(new PermissionRequirement("organisationalchart.Add")); });
                options.AddPolicy("organisationalchart.Edit", builder => { builder.AddRequirements(new PermissionRequirement("organisationalchart.Edit")); });
                options.AddPolicy("organisationalchart.Delete", builder => { builder.AddRequirements(new PermissionRequirement("organisationalchart.Delete")); });
                options.AddPolicy("organisationalchart.View", builder => { builder.AddRequirements(new PermissionRequirement("organisationalchart.View")); });
                options.AddPolicy("Leave.leavedetails.Add", builder => { builder.AddRequirements(new PermissionRequirement("Leave.leavedetails.Add")); });
                options.AddPolicy("Leave.leavedetails.Edit", builder => { builder.AddRequirements(new PermissionRequirement("Leave.leavedetails.Edit")); });
                options.AddPolicy("Leave.leavedetails.Delete", builder => { builder.AddRequirements(new PermissionRequirement("Leave.leavedetails.Delete")); });
                options.AddPolicy("Leave.leavedetails.View", builder => { builder.AddRequirements(new PermissionRequirement("Leave.leavedetails.View")); });
                options.AddPolicy("Leave.EmployeeLeave.Add", builder => { builder.AddRequirements(new PermissionRequirement("Leave.EmployeeLeave.Add")); });
                options.AddPolicy("Leave.EmployeeLeave.Edit", builder => { builder.AddRequirements(new PermissionRequirement("Leave.EmployeeLeave.Edit")); });
                options.AddPolicy("Leave.EmployeeLeave.Delete", builder => { builder.AddRequirements(new PermissionRequirement("Leave.EmployeeLeave.Delete")); });
                options.AddPolicy("Leave.EmployeeLeave.View", builder => { builder.AddRequirements(new PermissionRequirement("Leave.EmployeeLeave.View")); });
                options.AddPolicy("Leave.OfficeManagement.Add", builder => { builder.AddRequirements(new PermissionRequirement("Leave.OfficeManagement.Add")); });
                options.AddPolicy("Leave.OfficeManagement.Edit", builder => { builder.AddRequirements(new PermissionRequirement("Leave.OfficeManagement.Edit")); });
                options.AddPolicy("Leave.OfficeManagement.Delete", builder => { builder.AddRequirements(new PermissionRequirement("Leave.OfficeManagement.Delete")); });
                options.AddPolicy("Leave.OfficeManagement.View", builder => { builder.AddRequirements(new PermissionRequirement("Leave.OfficeManagement.View")); });
                options.AddPolicy("Leave.approveleavebyTeamlead.Approve", builder => { builder.AddRequirements(new PermissionRequirement("Leave.approveleavebyTeamlead.Approve")); });
                options.AddPolicy("Leave.approveleavebyOfficemanagement.Approve", builder => { builder.AddRequirements(new PermissionRequirement("Leave.approveleavebyOfficemanagement.Approve")); });
                options.AddPolicy("employeeprofile.personal.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.personal.Add")); });
                options.AddPolicy("employeeprofile.personal.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.personal.Edit")); });
                options.AddPolicy("employeeprofile.personal.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.personal.Delete")); });
                options.AddPolicy("employeeprofile.personal.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.personal.View")); });
                options.AddPolicy("employeeprofile.contact.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.contact.Add")); });
                options.AddPolicy("employeeprofile.contact.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.contact.Edit")); });
                options.AddPolicy("employeeprofile.contact.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.contact.Delete")); });
                options.AddPolicy("employeeprofile.contact.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.contact.View")); });
                options.AddPolicy("employeeprofile.identitycards.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.identitycards.Add")); });
                options.AddPolicy("employeeprofile.identitycards.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.identitycards.Edit")); });
                options.AddPolicy("employeeprofile.identitycards.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.identitycards.Delete")); });
                options.AddPolicy("employeeprofile.identitycards.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.identitycards.View")); });
                options.AddPolicy("employeeprofile.assets.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.assets.Add")); });
                options.AddPolicy("employeeprofile.assets.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.assets.Edit")); });
                options.AddPolicy("employeeprofile.assets.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.assets.Delete")); });
                options.AddPolicy("employeeprofile.assets.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.assets.View")); });
                options.AddPolicy("employeeprofile.education.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.education.Add")); });
                options.AddPolicy("employeeprofile.education.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.education.Edit")); });
                options.AddPolicy("employeeprofile.education.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.education.Delete")); });
                options.AddPolicy("employeeprofile.education.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.education.View")); });
                options.AddPolicy("employeeprofile.jobhistory.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.jobhistory.Add")); });
                options.AddPolicy("employeeprofile.jobhistory.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.jobhistory.Edit")); });
                options.AddPolicy("employeeprofile.jobhistory.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.jobhistory.Delete")); });
                options.AddPolicy("employeeprofile.jobhistory.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.jobhistory.View")); });
                options.AddPolicy("employeeprofile.document.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.document.Add")); });
                options.AddPolicy("employeeprofile.document.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.document.Edit")); });
                options.AddPolicy("employeeprofile.document.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.document.Delete")); });
                options.AddPolicy("employeeprofile.document.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.document.View")); });
                options.AddPolicy("employeeprofile.documentlist.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.documentlist.Add")); });
                options.AddPolicy("employeeprofile.documentlist.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.documentlist.Edit")); });
                options.AddPolicy("employeeprofile.documentlist.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.documentlist.Delete")); });
                options.AddPolicy("employeeprofile.documentlist.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.documentlist.View")); });
                options.AddPolicy("employeeprofile.TerminationofEmployment.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.TerminationofEmployment.Add")); });
                options.AddPolicy("employeeprofile.TerminationofEmployment.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.TerminationofEmployment.Edit")); });
                options.AddPolicy("employeeprofile.TerminationofEmployment.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.TerminationofEmployment.Delete")); });
                options.AddPolicy("employeeprofile.TerminationofEmployment.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.TerminationofEmployment.View")); });
                options.AddPolicy("employeeprofile.Salary.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Salary.Add")); });
                options.AddPolicy("employeeprofile.Salary.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Salary.Edit")); });
                options.AddPolicy("employeeprofile.Salary.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Salary.Delete")); });
                options.AddPolicy("employeeprofile.Salary.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Salary.View")); });
                options.AddPolicy("employeeprofile.Bonus.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Bonus.Add")); });
                options.AddPolicy("employeeprofile.Bonus.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Bonus.Edit")); });
                options.AddPolicy("employeeprofile.Bonus.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Bonus.Delete")); });
                options.AddPolicy("employeeprofile.Bonus.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.Bonus.View")); });
                options.AddPolicy("employeeprofile.consultantRate.Add", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.consultantRate.Add")); });
                options.AddPolicy("employeeprofile.consultantRate.Edit", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.consultantRate.Edit")); });
                options.AddPolicy("employeeprofile.consultantRate.Delete", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.consultantRate.Delete")); });
                options.AddPolicy("employeeprofile.consultantRate.View", builder => { builder.AddRequirements(new PermissionRequirement("employeeprofile.consultantRate.View")); });

            });


            services.AddTransient<IJobHistoryService, JobHistoryService>();
            services.AddTransient<IJobHistoryRepo, JobHistoryRepo>();

            services.AddTransient<IEducationService, EducationService>();
            services.AddTransient<IEducationRepo, EducationRepo>();

            services.AddTransient<IEmployeeTypeService, EmployeeTypeService>();
            services.AddTransient<IEmployeetypeRepo, EmployeeTypeRepo>();

            services.AddTransient<IContactService, ContactService>();
            services.AddTransient<IContactRepo, ContactRepo>();

            services.AddTransient<ITypeofEmploymentService, TypeofEmploymentService>();
            services.AddTransient<ITypeofEmploymentRepo, TypeofEmploymentRepo>();

            services.AddTransient<IEmploymentTypeService, EmploymentTypeService>();
            services.AddTransient<IEmploymentTypeRepo, EmploymentTypeRepo>();

            services.AddTransient<ItaxclassService, taxclassService>();
            services.AddTransient<ItaxclassRepo, taxclassRepo>();

            services.AddTransient<IMaritalStatusService, MaritalStatusService>();
            services.AddTransient<IMaritalStatusRepo, MaritalStatusRepo>();

            services.AddTransient<IEmployeenStatusService, EmployeenStatusService>();
            services.AddTransient<IEmployeenStatusRepo, EmployeenStatusRepo>();

            services.AddTransient<IEductionLevelService, EductionLevelService>();
            services.AddTransient<IEductionLevelRepo, EductionLevelRepo>();

            services.AddTransient<IDeliverymethodService, DeliverymethodService>();
            services.AddTransient<IDeliverymethodRepo, DeliverymethodRepo>();

            services.AddTransient<ILeaveTypeEmployeeService, LeaveTypeEmployeeService>();
            services.AddTransient<ILeaveTypeEmployeeRepo, LeaveTypeEmployeeRepo>();

            services.AddTransient<IDocumentListService, DocumentListService>();
            services.AddTransient<IDocumenListRepo, DocumentListRepo>();
        }
    }
}
