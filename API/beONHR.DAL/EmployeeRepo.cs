using beONHR.Entities.DTO;
using beONHR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities.Context;
using Azure;
using System.Net;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO.Enum;
using MimeKit;
using static beONHR.Entities.Permissions;
using System.Formats.Asn1;
using beONHR.Entities.User;
using System.Reflection;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;
using System.Collections.Immutable;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace beONHR.DAL
{
    public interface IEmployeeRepo
    {
        Task<ClientResponse> SaveEmployee(EmployeeDTO input); //, ICollection<LanguageCompetenceDTO> languageCompetences);
        Task<ClientResponse> GetEmployee();
        Task<ClientResponse> GetFilterEmployee(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteEmployee(Guid id);
        Task<ClientResponse> getEmployeeByHr(string id);
        Task<ClientResponse> GetEmployeeById(string id);
        Task<ClientResponse> GetEmployeeByLeader(string id);
        Task<ClientResponse> GetAvailableLeaveByEmployeeId(Guid employeeId);
    }
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly MainContext _context;
        private readonly IUserRepo _user;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly IEmailRepo _emailRepo;
        public EmployeeRepo(MainContext context, IUserRepo user, RoleManager<AspNetRoles> roleManager, IEmailRepo emailRepo)
        {
            _context = context;
            _user = user;
            _roleManager = roleManager;
            _emailRepo = emailRepo;
        }

        public async Task<ClientResponse> GetAvailableLeaveByEmployeeId(Guid employeeId)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var availableleave = await _context.EmployeeYearlyLeaveBalances
                                                  .Where(x => x.EmployeeId == employeeId && x.IsDeleted != true).Include(x=>x.LeaveType)
                                                  .Select(x => new EmployeeYearlyLeaveBalanceDTO
                                                  {
                                                      LeaveQuota = x.LeaveQuota,
                                                      LeaveName = x.LeaveType.TypeName,
                                                      
                                                  })
                                                  .ToListAsync();

                if (availableleave == null || !availableleave.Any())
                {
                    response.Message = "No AvailableLeave Found for the Employee";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "AvailableLeave Retrieved Successfully";
                    response.HttpResponse = availableleave;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> SaveEmployee(EmployeeDTO input) //, ICollection<LanguageCompetenceDTO> languageCompetences)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {

                    var employee = await _context.Employees
                        .Where(x => x.EmployeeNumber == input.EmployeeNumber &&
                                                    x.CurrentStatusId == input.CurrentStatusId &&
                                                    x.EmployementTypeId == input.EmployementTypeId &&
                                                    x.TypeofEmploymentId == input.TypeofEmploymentId &&
                                                    x.WorkingHours == input.WorkingHours &&
                                                    x.JobTitle == input.JobTitle &&
                                                    x.ContractualStartDate == input.ContractualStartDate &&
                                                    x.ContractualEndDate == input.ContractualEndDate &&
                                                    x.Email == input.Email &&
                                                    x.FirstName == input.FirstName &&
                                                    x.MiddleName == input.MiddleName &&
                                                    x.LastName == input.LastName &&
                                                    x.Gender == input.Gender &&
                                                    x.Birthdate == input.Birthdate &&
                                                    x.BirthCity == input.BirthCity &&
                                                    x.BirthCountryId == input.BirthCountryId &&
                                                    x.Nationality == input.Nationality &&
                                                    x.TaxId == input.TaxId &&
                                                    x.SocialSecurity == input.SocialSecurity &&
                                                    x.TaxClassId == input.TaxClassId &&
                                                    x.HealthInsaurance == input.HealthInsaurance &&
                                                    x.MaritalStatusId == input.MaritalStatusId &&
                                                    x.Religiousaffiliation == input.Religiousaffiliation &&
                                                    x.ProfilePhoto == input.ProfilePhoto &&
                                                    x.PersonalSheet == input.PersonalSheet &&
                                                    x.SocialSecurityFile == input.SocialSecurityFile &&
                                                    x.EmployeeStatusId == input.EmployeeStatusId &&
                                                    //*********Children information**********//
                                                    x.employeehavechildren == input.employeehavechildren &&
                                                    x.child_FirstName == input.child_FirstName &&
                                                    x.FamilyName == input.FamilyName &&
                                                    x.CHild_BirthDate == input.CHild_BirthDate &&
                                                    x.Locationchildregistered == input.Locationchildregistered &&
                                                    x.socialcareinsurance == input.socialcareinsurance &&
                                                    x.BirthCertificate == input.BirthCertificate &&
                                                    //*********Job Title and Role**********//
                                                    x.SLGStatus == input.SLGStatus &&
                                                    x.Designation == input.Designation &&
                                                    x.RoleId == input.RoleId &&
                                                    //*********Leadership**********//
                                                    x.Leader1Id == input.Leader1Id &&
                                                    x.Leader2Id == input.Leader2Id &&
                                                    x.DefaulLeaderId == input.DefaulLeaderId &&
                                                    //*********Probation**********//
                                                    x.Istheemployeeonprobation == input.Istheemployeeonprobation &&
                                                    x.StartDate == input.StartDate &&
                                                    x.EndDate == input.EndDate &&
                                                    x.Prob_AdjustedEndDate == input.Prob_AdjustedEndDate &&
                                                    x.AdjustedDocument == input.AdjustedDocument &&
                                                    x.adjustedenddatecheck == input.adjustedenddatecheck &&
                                                    x.ProbationUnlimited == input.ProbationUnlimited &&
                                                    //*********End of Employment**********//
                                                    x.Isthistheendofemployment == input.Isthistheendofemployment &&
                                                    x.NoticePeriodEndDate == input.NoticePeriodEndDate &&
                                                    x.NoticePeriodStartDate == input.NoticePeriodStartDate &&
                                                    //*********Termination of Employment**********//
                                                    x.IsTerminated == input.IsTerminated &&
                                                    x.TerminationStartDate == input.TerminationStartDate &&
                                                    x.TerminationEndDate == input.TerminationEndDate &&
                                                    x.Terminationofemplyee == input.Terminationofemplyee &&
                                                    x.Dateofreceipt == input.Dateofreceipt &&
                                                    x.DeliverymethodId == input.DeliverymethodId &&
                                                    x.IsDeleted != true).ToListAsync();

                    if (employee.Count() == 0)
                    {
                        Employee model = new()
                        {
                            Id = Guid.NewGuid(),
                            EmployeeNumber = input.EmployeeNumber,
                            CurrentStatusId = input.CurrentStatusId,
                            EmployementTypeId = input.EmployementTypeId,
                            TypeofEmploymentId = input.TypeofEmploymentId,
                            WorkingHours = input.WorkingHours,
                            JobTitle = input.JobTitle,
                            ContractualStartDate = input.ContractualStartDate,
                            ContractualEndDate = input.ContractualEndDate,
                            Email = input.Email,
                            FirstName = input.FirstName,
                            MiddleName = input.MiddleName,
                            LastName = input.LastName,
                            Gender = input.Gender,
                            Birthdate = input.Birthdate,
                            BirthCity = input.BirthCity,
                            BirthCountryId = input.BirthCountryId,
                            Nationality = input.Nationality,
                            TaxId = input.TaxId,
                            SocialSecurity = input.SocialSecurity,
                            TaxClassId = input.TaxClassId,
                            HealthInsaurance = input.HealthInsaurance,
                            MaritalStatusId = input.MaritalStatusId,
                            Religiousaffiliation = input.Religiousaffiliation,
                            ProfilePhoto = input.ProfilePhoto, //A FIle
                            PersonalSheet = input.PersonalSheet,//A FIle
                            SocialSecurityFile = input.SocialSecurityFile, //A FIle
                            EmployeeStatusId = input.EmployeeStatusId,
                            //*********Children information**********//
                            employeehavechildren = input.employeehavechildren,
                            child_FirstName = input.child_FirstName,
                            FamilyName = input.FamilyName,
                            CHild_BirthDate = input.CHild_BirthDate,
                            Locationchildregistered = input.Locationchildregistered,
                            socialcareinsurance = input.socialcareinsurance, //A FIle
                            BirthCertificate = input.BirthCertificate, //A FIle
                            //*********Language Competence**********//
                            LanguageCompetences = input.LanguageCompetences.Select(c => new LanguageCompetence
                            {
                                Id = Guid.NewGuid(),
                                Name = c.Name,
                                Level = c.Level,
                                LanguagesCertificate = c.LanguagesCertificate //A FIle
                            }).ToImmutableList(),
                            //*********LeaveType**********//
                            EmployeeYearlyLeaveBalances = input.EmployeeYearlyLeaveBalances.Select(c => new EmployeeYearlyLeaveBalance
                            {
                                Id = Guid.NewGuid(),
                                LeaveStartDate = c.LeaveStartDate,
                                LeaveEndDate = c.LeaveEndDate,
                                LeaveTypeEmployee = c.LeaveTypeEmployee,
                                LeaveTypesEmployee = c.LeaveTypesEmployee,
                                LeaveQuota = c.LeaveQuota,
                                AdjustedEndDate = c.AdjustedEndDate,
                            }).ToList(),
                            //*********Job Title and Role**********//
                            SLGStatus = input.SLGStatus,
                            Designation = input.Designation,
                            RoleId = input.RoleId,
                            //*********Leadership**********//
                            Leader1Id = input.Leader1Id,
                            Leader2Id = input.Leader2Id,
                            DefaulLeaderId = input.DefaulLeaderId,
                            //*********Probation**********//
                            Istheemployeeonprobation = input.Istheemployeeonprobation,
                            StartDate = input.StartDate,
                            EndDate = input.EndDate,
                            Prob_AdjustedEndDate = input.Prob_AdjustedEndDate,
                            AdjustedDocument = input.AdjustedDocument,
                            adjustedenddatecheck = input.adjustedenddatecheck, //A FIle
                            ProbationUnlimited = input.ProbationUnlimited,
                            //*********End of Employment**********//
                            Isthistheendofemployment = input.Isthistheendofemployment,
                            NoticePeriodEndDate = input.NoticePeriodEndDate,
                            NoticePeriodStartDate = input.NoticePeriodStartDate,
                            //*********Termination of Employment**********//
                            IsTerminated = input.IsTerminated,
                            TerminationStartDate = input.TerminationStartDate,
                            TerminationEndDate = input.TerminationEndDate,
                            Terminationofemplyee = input.Terminationofemplyee, //A FIle
                            Dateofreceipt = input.Dateofreceipt,
                            DeliverymethodId = input.DeliverymethodId,
                        };


                        //List<DocumentList> documentLists = new List<DocumentList>();
                        //if (!string.IsNullOrEmpty(input.ProfilePhoto))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Genaral",
                        //        FileName = "ProfilePhoto",
                        //        Documents = input.ProfilePhoto
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.PersonalSheet))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Genaral",
                        //        FileName = "PersonalSheet",
                        //        Documents = input.PersonalSheet
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.SocialSecurityFile))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Genaral",
                        //        FileName = "SocialSecurityFile",
                        //        Documents = input.SocialSecurityFile
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.socialcareinsurance))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Children information",
                        //        FileName = "socialcareinsurance",
                        //        Documents= input.socialcareinsurance
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.BirthCertificate))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Children Information",
                        //        FileName = "BirthCertificate",
                        //        Documents = input.BirthCertificate
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.AdjustedDocument))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Probation",
                        //        FileName = "AdjustedDocument",
                        //        Documents = input.AdjustedDocument
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.Terminationofemplyee))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        EmployeeId = model.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Termination",
                        //        FileName = "Termination Document",
                        //        Documents = input.Terminationofemplyee
                        //    });
                        //}
                        //model.DocumentList = documentLists;


                        await _context.Employees.AddAsync(model);


                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "Employee not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            var roleName = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                            Register register = new Register()
                            {
                                username = model.Id.ToString(),
                                password = GenrateEmployeePassword(model),
                                Email = model.Email,
                                Role = roleName.ToString(),
                            };

                            var userRes = await _user.RegisterUser(register);
                            if (userRes.IsSuccess == true)
                            {
                                response.Message = "Employee inserted successfully";
                                response.HttpResponse = model.Id;
                                response.IsSuccess = true;
                                response.StatusCode = HttpStatusCode.OK;


                                forgotmail forgotmail = new forgotmail()
                                {
                                    Email = model.Email,
                                    FirstName = model.FirstName
                                };

                                await _emailRepo.GenerateForgotPasswordTokenAsync(forgotmail);

                            }
                        }
                    }
                    else
                    {
                        response.Message = "Employee already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }

                    return response;
                }
                else
                {
                    var existingEmployee = await _context.Employees
                                            .Include(e => e.LanguageCompetences)
                                            .Include(b=>b.EmployeeYearlyLeaveBalances)
                                            .Include(e => e.DocumentList)
                                            .FirstOrDefaultAsync(x => x.EmployeeNumber == input.EmployeeNumber && x.IsDeleted == false);

                    if (existingEmployee != null)
                    {
                        existingEmployee.EmployeeNumber = input.EmployeeNumber;
                        existingEmployee.CurrentStatusId = input.CurrentStatusId;
                        existingEmployee.EmployementTypeId = input.EmployementTypeId;
                        existingEmployee.TypeofEmploymentId = input.TypeofEmploymentId;
                        existingEmployee.WorkingHours = input.WorkingHours;
                        existingEmployee.JobTitle = input.JobTitle;
                        existingEmployee.ContractualStartDate = input.ContractualStartDate;
                        existingEmployee.ContractualEndDate = input.ContractualEndDate;
                        existingEmployee.Email = input.Email;
                        existingEmployee.FirstName = input.FirstName;
                        existingEmployee.MiddleName = input.MiddleName;
                        existingEmployee.LastName = input.LastName;
                        existingEmployee.Gender = input.Gender;
                        existingEmployee.Birthdate = input.Birthdate;
                        existingEmployee.BirthCity = input.BirthCity;
                        existingEmployee.BirthCountryId = input.BirthCountryId;
                        existingEmployee.Nationality = input.Nationality;
                        existingEmployee.TaxId = input.TaxId;
                        existingEmployee.SocialSecurity = input.SocialSecurity;
                        existingEmployee.TaxClassId = input.TaxClassId;
                        existingEmployee.HealthInsaurance = input.HealthInsaurance;
                        existingEmployee.MaritalStatusId = input.MaritalStatusId;
                        existingEmployee.Religiousaffiliation = input.Religiousaffiliation;
                        existingEmployee.ProfilePhoto = input.ProfilePhoto;
                        existingEmployee.PersonalSheet = input.PersonalSheet;
                        existingEmployee.SocialSecurityFile = input.SocialSecurityFile;
                        existingEmployee.EmployeeStatusId = input.EmployeeStatusId;

                        // Children information
                        existingEmployee.employeehavechildren = input.employeehavechildren;
                        existingEmployee.child_FirstName = input.child_FirstName;
                        existingEmployee.FamilyName = input.FamilyName;
                        existingEmployee.CHild_BirthDate = input.CHild_BirthDate;
                        existingEmployee.Locationchildregistered = input.Locationchildregistered;
                        existingEmployee.socialcareinsurance = input.socialcareinsurance;
                        existingEmployee.BirthCertificate = input.BirthCertificate;

                        if (existingEmployee.LanguageCompetences != null && existingEmployee.LanguageCompetences.Count() != 0)
                        {

                            _context.LanguageCompetences.RemoveRange(existingEmployee.LanguageCompetences);
                        }
                        // Language Competence
                        existingEmployee.LanguageCompetences = input.LanguageCompetences.Select(c => new LanguageCompetence
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Level = c.Level,
                            LanguagesCertificate = c.LanguagesCertificate
                        }).ToList();

                        if (existingEmployee.EmployeeYearlyLeaveBalances != null && existingEmployee.EmployeeYearlyLeaveBalances.Count() != 0)
                        {
                            _context.EmployeeYearlyLeaveBalances.RemoveRange(existingEmployee.EmployeeYearlyLeaveBalances);
                        }       
                        // LeaveType
                        existingEmployee.EmployeeYearlyLeaveBalances = input.EmployeeYearlyLeaveBalances.Select(c => new EmployeeYearlyLeaveBalance
                        {
                            Id = c.Id,
                            LeaveStartDate = c.LeaveStartDate,
                            LeaveEndDate = c.LeaveEndDate,
                            LeaveTypeEmployee = c.LeaveTypeEmployee,
                            LeaveTypesEmployee = c.LeaveTypesEmployee,
                            LeaveQuota = c.LeaveQuota,
                            AdjustedEndDate = c.AdjustedEndDate,
                        }).ToList();

                        // Job Title and Role
                        existingEmployee.SLGStatus = input.SLGStatus;
                        existingEmployee.Designation = input.Designation;
                        existingEmployee.RoleId = input.RoleId;

                        // Leadership
                        existingEmployee.Leader1Id = input.Leader1Id;
                        existingEmployee.Leader2Id = input.Leader2Id;
                        existingEmployee.DefaulLeaderId = input.DefaulLeaderId;

                        // Probation
                        existingEmployee.Istheemployeeonprobation = input.Istheemployeeonprobation;
                        existingEmployee.StartDate = input.StartDate;
                        existingEmployee.EndDate = input.EndDate;
                        existingEmployee.Prob_AdjustedEndDate = input.Prob_AdjustedEndDate;
                        existingEmployee.AdjustedDocument = input.AdjustedDocument;
                        existingEmployee.adjustedenddatecheck = input.adjustedenddatecheck;
                        existingEmployee.ProbationUnlimited = input.ProbationUnlimited;

                        // End of Employment
                        existingEmployee.Isthistheendofemployment = input.Isthistheendofemployment;
                        existingEmployee.NoticePeriodEndDate = input.NoticePeriodEndDate;
                        existingEmployee.NoticePeriodStartDate = input.NoticePeriodStartDate;

                        // Termination of Employment
                        existingEmployee.IsTerminated = input.IsTerminated;
                        existingEmployee.TerminationStartDate = input.TerminationStartDate;
                        existingEmployee.TerminationEndDate = input.TerminationEndDate;
                        existingEmployee.Terminationofemplyee = input.Terminationofemplyee;
                        existingEmployee.Dateofreceipt = input.Dateofreceipt;
                        existingEmployee.DeliverymethodId = input.DeliverymethodId;


                        //existingEmployee.DocumentList.Clear();
                        //List<DocumentList> documentLists = new List<DocumentList>();
                        //if (!string.IsNullOrEmpty(input.ProfilePhoto))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Genaral",
                        //        FileName = "ProfilePhoto",
                        //        Documents = input.ProfilePhoto
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.PersonalSheet))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Genaral",
                        //        FileName = "PersonalSheet",
                        //        Documents = input.PersonalSheet
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.SocialSecurityFile))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Genaral",
                        //        FileName = "SocialSecurityFile",
                        //        Documents = input.SocialSecurityFile
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.socialcareinsurance))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Children information",
                        //        FileName = "socialcareinsurance",
                        //        Documents = input.socialcareinsurance
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.BirthCertificate))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Children Information",
                        //        FileName = "BirthCertificate",
                        //        Documents = input.BirthCertificate
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.AdjustedDocument))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Probation",
                        //        FileName = "AdjustedDocument",
                        //        Documents = input.AdjustedDocument
                        //    });
                        //}
                        //if (!string.IsNullOrEmpty(input.Terminationofemplyee))
                        //{
                        //    documentLists.Add(new DocumentList
                        //    {
                        //        EmployeeId = existingEmployee.Id,
                        //        TabName = "Personal",
                        //        Modulename = "Termination",
                        //        FileName = "Termination Document",
                        //        Documents = input.Terminationofemplyee
                        //    });
                        //}
                        //existingEmployee.DocumentList = documentLists;


                        _context.Employees.Update(existingEmployee);


                        await _context.SaveChangesAsync();



                        response.Message = "Employee updated successfully";
                        response.HttpResponse = existingEmployee.Id;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "Employee not found";
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.IsSuccess = false;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred while saving the employee";
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                throw ex;
            }
        }


        private string GenrateEmployeePassword(Employee emp)
        {
            var password = "";
            if (emp != null)
            {
                password = emp.FirstName.First().ToString().ToUpper() + String.Join("", emp.FirstName.Skip(1)) + "@" + emp.EmployeeNumber;
            }

            return password;
        }


        public async Task<ClientResponse> GetEmployee()
        {
            ClientResponse response = new();
            try
            {
                var employees = await _context.Employees
                 .Where(x => x.IsDeleted != true).Include(x => x.Role).Include(x => x.EmploymentType).OrderBy(x => x.FirstName).Include(x => x.EmployeenStatus)
                 .Select(x => new EmployeeDTO
                 {
                     Id = x.Id,
                     EmployeeNumber = x.EmployeeNumber,
                     CurrentStatusId = x.CurrentStatusId,
                     EmployementTypeId = x.EmployementTypeId,
                     TypeofEmploymentId = x.TypeofEmploymentId,
                     WorkingHours = x.WorkingHours,
                     JobTitle = x.JobTitle,
                     ContractualStartDate = x.ContractualStartDate,
                     ContractualEndDate = x.ContractualEndDate,
                     Email = x.Email,
                     FullName = $"{x.FirstName} {x.MiddleName} {x.LastName}", // Combine first, middle, and last names into a single field
                     Gender = x.Gender,
                     Birthdate = x.Birthdate,
                     BirthCity = x.BirthCity,
                     BirthCountryId = x.BirthCountryId,
                     Nationality = x.Nationality,
                     TaxId = x.TaxId,
                     SocialSecurity = x.SocialSecurity,
                     TaxClassId = x.TaxClassId,
                    // HealthInsaurance = x.HealthInsaurance,
                     MaritalStatusId = x.MaritalStatusId,
                     Religiousaffiliation = x.Religiousaffiliation,
                    // ProfilePhoto = x.ProfilePhoto,
                    // PersonalSheet = x.PersonalSheet,
                    //  SocialSecurityFile = x.SocialSecurityFile,
                     EmployeeStatusId = x.EmployeeStatusId,

                     // Children information
                     //employeehavechildren = x.employeehavechildren,
                     //child_FirstName = x.child_FirstName,
                     //FamilyName = x.FamilyName,
                     //CHild_BirthDate = x.CHild_BirthDate,
                     //Locationchildregistered = x.Locationchildregistered,
                     //socialcareinsurance = x.socialcareinsurance,
                     //BirthCertificate = x.BirthCertificate,
                     // Language Competence
                     //LanguageCompetences = x.LanguageCompetences.Select(c => new LanguageCompetenceDTO
                     //{
                     //    Id = c.Id,
                     //    Name = c.Name,
                     //    Level = c.Level,
                     //    LevelName=c.LanguageLevel.Level,
                     //    LanguagesCertificate = c.LanguagesCertificate
                     //}).ToList(),
                     // LeaveType
                     //EmployeeYearlyLeaveBalances = x.EmployeeYearlyLeaveBalances.Select(c => new EmployeeYearlyLeaveBalanceDTO
                     //{
                     //    Id = c.Id,
                     //    LeaveStartDate = c.LeaveStartDate,
                     //    LeaveEndDate = c.LeaveEndDate,
                     //    LeaveTypeEmployee = c.LeaveTypeEmployee,
                     //    LeaveTypesEmployee = c.LeaveTypesEmployee,
                         
                     //    LeaveQuota = c.LeaveQuota,
                     //    AdjustedEndDate = c.AdjustedEndDate,
                     //}).ToList(),
                     // Job Title and Role
                     SLGStatus = x.SLGStatus,
                     SLGStatusname = x.SLGGroup.InitialStatus,
                     Designation = x.Designation,
                     RoleId = x.RoleId,
                     Rolename = x.Role.Name,
                     // Leadership
                     Leader1Id = x.Leader1Id,
                     Leader2Id = x.Leader2Id,
                     DefaulLeaderId = x.DefaulLeaderId,
                     // Probation
                     //Istheemployeeonprobation = x.Istheemployeeonprobation,
                     //StartDate = x.StartDate,
                     //EndDate = x.EndDate,
                     //Prob_AdjustedEndDate = x.Prob_AdjustedEndDate,
                     //AdjustedDocument = x.AdjustedDocument,
                     //adjustedenddatecheck = x.adjustedenddatecheck,
                     //ProbationUnlimited = x.ProbationUnlimited,
                     // End of Employment
                     //Isthistheendofemployment = x.Isthistheendofemployment,
                     //NoticePeriodEndDate = x.NoticePeriodEndDate,
                     //NoticePeriodStartDate = x.NoticePeriodStartDate,
                     //Termination of Employment
                     //IsTerminated = x.IsTerminated,
                     //TerminationStartDate = x.TerminationStartDate,
                     //TerminationEndDate = x.TerminationEndDate,
                     //Terminationofemplyee = x.Terminationofemplyee,
                     //Dateofreceipt = x.Dateofreceipt,
                     //DeliverymethodId = x.DeliverymethodId,
                 })
             .ToListAsync();

                if (employees == null || employees.Count == 0)
                {
                    response.Message = "No employees found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Employees retrieved successfully";
                    response.HttpResponse = employees;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetEmployeeById(string id)
        {
            ClientResponse response = new();
            try
            {
                Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
                List<EmployeeLeave> allLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> pendingLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> leaveHistory = new List<EmployeeLeave>();


                if (guidRegex.IsMatch(id))
                {

                    var employees = await _context.Employees
                .Where(x => x.IsDeleted != true && x.Id == new Guid(id)).Include(x => x.Role)
                .Select(x => new EmployeeDTO
                {
                    Id = x.Id,
                    EmployeeNumber = x.EmployeeNumber,
                    CurrentStatusId = x.CurrentStatusId,
                    EmployementTypeId = x.EmployementTypeId,
                    TypeofEmploymentId = x.TypeofEmploymentId,
                    WorkingHours = x.WorkingHours,
                    JobTitle = x.JobTitle,
                    ContractualStartDate = x.ContractualStartDate,
                    ContractualEndDate = x.ContractualEndDate,
                    Email = x.Email,
                    FirstName=x.FirstName,
                    LastName=x.LastName,
                    MiddleName=x.MiddleName,
                    FullName = $"{x.FirstName} {x.MiddleName} {x.LastName}", // Combine first, middle, and last names into a single field
                    Gender = x.Gender,
                    Birthdate = x.Birthdate,
                    BirthCity = x.BirthCity,
                    BirthCountryId = x.BirthCountryId,
                    Nationality = x.Nationality,
                    TaxId = x.TaxId,
                    SocialSecurity = x.SocialSecurity,
                    TaxClassId = x.TaxClassId,
                    HealthInsaurance = x.HealthInsaurance,
                    MaritalStatusId = x.MaritalStatusId,
                    Religiousaffiliation = x.Religiousaffiliation,
                    ProfilePhoto = x.ProfilePhoto,
                    PersonalSheet = x.PersonalSheet,
                    SocialSecurityFile = x.SocialSecurityFile,
                    EmployeeStatusId = x.EmployeeStatusId,
                    // Children information
                    employeehavechildren = x.employeehavechildren,
                    child_FirstName = x.child_FirstName,
                    FamilyName = x.FamilyName,
                    CHild_BirthDate = x.CHild_BirthDate,
                    Locationchildregistered = x.Locationchildregistered,
                    socialcareinsurance = x.socialcareinsurance,
                    BirthCertificate = x.BirthCertificate,
                    // Language Competence
                    LanguageCompetences = x.LanguageCompetences.Select(c => new LanguageCompetenceDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Level = c.Level,
                        LevelName = c.LanguageLevel.Level,
                        LanguagesCertificate = c.LanguagesCertificate
                       
                    }).ToList(),
                    // LeaveType
                    EmployeeYearlyLeaveBalances = x.EmployeeYearlyLeaveBalances.Select(c => new EmployeeYearlyLeaveBalanceDTO
                    {
                        Id = c.Id,
                        LeaveStartDate = c.LeaveStartDate,
                        LeaveEndDate = c.LeaveEndDate,
                        LeaveTypeEmployee = c.LeaveTypeEmployee,
                        LeaveName=c.LeaveType.TypeName,
                        LeaveTypesEmployee = c.LeaveTypesEmployee,
                        LeaveQuota = c.LeaveQuota,
                        AdjustedEndDate = c.AdjustedEndDate,
                    }).ToList(),
                    // Job Title and Role
                    SLGStatus = x.SLGStatus,
                    SLGStatusname = x.SLGGroup.InitialStatus,
                    Designation = x.Designation,
                    RoleId = x.RoleId,
                    Rolename = x.Role.Name,
                    // Leadership
                    Leader1Id = x.Leader1Id,
                    Leader2Id = x.Leader2Id,
                    DefaulLeaderId = x.DefaulLeaderId,
                    //Probation
                    Istheemployeeonprobation = x.Istheemployeeonprobation,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Prob_AdjustedEndDate = x.Prob_AdjustedEndDate,
                    AdjustedDocument = x.AdjustedDocument,
                    adjustedenddatecheck = x.adjustedenddatecheck,
                    ProbationUnlimited = x.ProbationUnlimited,
                    // End of Employment
                    Isthistheendofemployment = x.Isthistheendofemployment,
                    NoticePeriodEndDate = x.NoticePeriodEndDate,
                    NoticePeriodStartDate = x.NoticePeriodStartDate,
                    // Termination of Employment
                    IsTerminated = x.IsTerminated,
                    TerminationStartDate = x.TerminationStartDate,
                    TerminationEndDate = x.TerminationEndDate,
                    Terminationofemplyee = x.Terminationofemplyee,
                    Dateofreceipt = x.Dateofreceipt,
                    DeliverymethodId = x.DeliverymethodId,
                })
            .FirstOrDefaultAsync();

                    if (employees == null)
                    {
                        response.Message = "No employees found";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "Employees retrieved successfully";
                        response.HttpResponse = employees;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {

                    response.Message = "No employees found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;

                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteEmployee(Guid id)
        {
            ClientResponse response = new();
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.LanguageCompetences)
                    .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != true);

                if (employee != null)
                {
                    // Mark the employee as deleted
                    employee.IsDeleted = true;

                    // Mark all associated language competences as deleted
                    foreach (var competence in employee.LanguageCompetences)
                    {
                        competence.IsDeleted = true;
                    }

                    // Update the employee and associated language competences
                    _context.Employees.Update(employee);
                    _context.LanguageCompetences.UpdateRange(employee.LanguageCompetences);

                    // Save changes to the database
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "Employee deletion failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Employee deleted successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Employee not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetEmployeeByLeader(string id)
        {
            ClientResponse response = new();
            try
            {
                Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
                List<EmployeeLeave> allLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> pendingLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> leaveHistory = new List<EmployeeLeave>();


                if (guidRegex.IsMatch(id))
                {

                    var employees = await _context.Employees
                   .Where(x => x.IsDeleted != true && (x.Leader1Id == new Guid(id) || x.Leader2Id == new Guid(id) || x.DefaulLeaderId == new Guid(id))).Include(x => x.Role)
                   .Select(x => new EmployeeDTO
                   {
                       Id = x.Id,
                       EmployeeNumber = x.EmployeeNumber,
                       Email = x.Email,
                       FullName = $"{x.FirstName} {x.MiddleName} {x.LastName}", // Combine first, middle, and last names into a single field
                       // LeaveType
                       EmployeeYearlyLeaveBalances = x.EmployeeYearlyLeaveBalances.Select(c => new EmployeeYearlyLeaveBalanceDTO
                       {
                           Id = c.Id,
                           LeaveStartDate = c.LeaveStartDate,
                           LeaveEndDate = c.LeaveEndDate,
                           LeaveTypeEmployee = c.LeaveTypeEmployee,
                           LeaveTypesEmployee = c.LeaveTypesEmployee,
                           LeaveQuota = c.LeaveQuota,
                           AdjustedEndDate = c.AdjustedEndDate,
                       }).ToList(),
                       // Job Title and Role
                       SLGStatus = x.SLGStatus,
                       Designation = x.Designation,
                       RoleId = x.RoleId,
                       // Leadership
                       Leader1Id = x.Leader1Id,
                       Leader2Id = x.Leader2Id,
                       DefaulLeaderId = x.DefaulLeaderId,
                   })
               .ToListAsync();

                    if (employees == null)
                    {
                        response.Message = "No employees found";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "Employees retrieved successfully";
                        response.HttpResponse = employees.OrderBy(x => x.FirstName).ToList();
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {

                    response.Message = "No employees found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;

                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> getEmployeeByHr(string id)
        {
            ClientResponse response = new();
            try
            {
                Regex guidRegex = new Regex(@"^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$");
                List<EmployeeLeave> allLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> pendingLeave = new List<EmployeeLeave>();
                List<EmployeeLeave> leaveHistory = new List<EmployeeLeave>();

                if (guidRegex.IsMatch(id))
                {

                    var employees = await _context.Employees
                   .Where(x => x.IsDeleted != true && x.Id != new Guid(id) && !x.SLGGroup.StatusName.Contains("SLG 1")).Include(x => x.Role).Include(x => x.SLGGroup)
                   .Select(x => new EmployeeDTO
                   {
                       Id = x.Id,
                       EmployeeNumber = x.EmployeeNumber,
                       Email = x.Email,
                       FullName = $"{x.FirstName} {x.MiddleName} {x.LastName}", // Combine first, middle, and last names into a single field
                       // LeaveType
                       EmployeeYearlyLeaveBalances = x.EmployeeYearlyLeaveBalances.Select(c => new EmployeeYearlyLeaveBalanceDTO
                       {
                           Id = c.Id,
                           LeaveStartDate = c.LeaveStartDate,
                           LeaveEndDate = c.LeaveEndDate,
                           LeaveTypeEmployee = c.LeaveTypeEmployee,
                           LeaveTypesEmployee = c.LeaveTypesEmployee,
                           LeaveQuota = c.LeaveQuota,
                           AdjustedEndDate = c.AdjustedEndDate,
                       }).ToList(),
                       // Job Title and Role
                       SLGStatus = x.SLGStatus,
                       Designation = x.Designation,
                       RoleId = x.RoleId,
                       // Leadership
                       Leader1Id = x.Leader1Id,
                       Leader2Id = x.Leader2Id,
                       DefaulLeaderId = x.DefaulLeaderId,
                   }).ToListAsync();

                    if (employees == null)
                    {
                        response.Message = "No employees found";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "Employees retrieved successfully";
                        response.HttpResponse = employees.OrderBy(x => x.FirstName).ToList();
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {

                    response.Message = "No employees found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;

                }
                return response;



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///filter
        static Expression<Func<EmployeeDTO, bool>> CombineLambdas(Expression<Func<EmployeeDTO, bool>> expr1, Expression<Func<EmployeeDTO, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(EmployeeDTO));
            Expression body = null;
            if (filterRequset.filterConditionAndOr == (int)filterConditionAndOrEnum.OrCondition)
            {
                body = Expression.OrElse(
                 expr1.Body.ReplaceParameter(expr1.Parameters[0], parameter),
                 expr2.Body.ReplaceParameter(expr2.Parameters[0], parameter)
                );
            }
            else
            {
                body = Expression.AndAlso(
                                expr1.Body.ReplaceParameter(expr1.Parameters[0], parameter),
                                expr2.Body.ReplaceParameter(expr2.Parameters[0], parameter)
                               );
            }

            return Expression.Lambda<Func<EmployeeDTO, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterEmployee(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {

                //var query = (from e in _context.Employees
                //                 join c in _context.Contacts on e.Id equals c.EmployeeId
                //                 join ca in _context.ContactAdresses on c.Id equals ca.ContactId
                //                 select new EmployeeDTO
                //                 {
                //                     Id = e.Id,
                //                     EmployeeNumber = e.EmployeeNumber,
                //                     EmployementTypeId = e.EmployementTypeId,
                //                     EmployementTypename = e.EmploymentType.employmenttype,
                //                     ContractualStartDate = e.ContractualStartDate,
                //                     FirstName = e.FirstName,
                //                     MiddleName = e.MiddleName,
                //                     LastName = e.LastName,
                //                     EmployeeStatusId = e.EmployeeStatusId,
                //                     EmployeeStatusname = e.EmployeenStatus.employeenstatus,
                //                     Rolename = e.Role.Name,
                //                     RoleId = e.RoleId,
                //                     OfficeEmail = ca.ContactEmailPrivate,
                //                     WorkLocation=ca.ContactZipCode,
                //                     PhoneNumber=ca.ContactPhone1,
                //                 }).AsQueryable();


                var query = (from e in _context.Employees
                             join c in _context.Contacts on e.Id equals c.EmployeeId into contacts
                             from contact in contacts.DefaultIfEmpty()
                             join ca in _context.ContactAdresses on contact != null ? contact.Id : (Guid?)null equals ca.ContactId into addresses
                             from address in addresses.DefaultIfEmpty()
                             select new EmployeeDTO
                             {
                                 Id = e.Id,
                                 EmployeeNumber = e.EmployeeNumber,
                                 EmployementTypeId = e.EmployementTypeId,
                                 EmployementTypename = e.EmploymentType.employmenttype,
                                 ContractualStartDate = e.ContractualStartDate,
                                 FullName = $"{e.FirstName} {e.MiddleName} {e.LastName}", // Combine first, middle, and last names into a single field
                                 MiddleName = e.MiddleName,
                                 EmployeeStatusId = e.EmployeeStatusId,
                                 EmployeeStatusname = e.EmployeenStatus.employeenstatus,
                                 Rolename = e.Role != null ? e.Role.Name : null,
                                 RoleId = e.RoleId,
                                 OfficeEmail = address != null ? address.ContactEmailPrivate : null,
                                 WorkLocation = address != null ? address.ContactZipCode : null,
                                 PhoneNumber = address != null ? address.ContactPhone1 : null,
                             }).AsQueryable();


                //var phone= _context.Contacts.Where(x=>x.IsDeleted).Include(x=>x.ContactAdressDetails).Select(x=>x.ContactAdressDetails.Select(x=>x.ContactPhone1)).FirstOrDefault();
                //var query = _context.Employees.Where(x => x.IsDeleted != true).Include(x => x.Role).Include(x => x.EmploymentType).Include(x => x.EmployeenStatus).Select(x => new EmployeeDTO
                //{
                //    Id = x.Id,
                //    EmployeeNumber = x.EmployeeNumber,
                //    EmployementTypeId = x.EmployementTypeId,
                //    EmployementTypename = x.EmploymentType.employmenttype,
                //    ContractualStartDate = x.ContractualStartDate,
                //    FirstName = x.FirstName,
                //    MiddleName = x.MiddleName,
                //    LastName = x.LastName,
                //    EmployeeStatusId = x.EmployeeStatusId,
                //    EmployeeStatusname = x.EmployeenStatus.employeenstatus,
                //    Rolename = x.Role.Name,
                //    RoleId = x.RoleId,
                //    OfficeEmail = _context.Contacts.Where(y => y.EmployeeId == x.Id).Select(x => x.ContactAdressDetails.Select(x => x.ContactPhone1)).FirstOrDefault().ToString(),
                //}).AsQueryable();
                // Loop through each filter

                Expression<Func<EmployeeDTO, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(EmployeeDTO));
                Expression<Func<EmployeeDTO, bool>> lambda = null;

                if (filterRequset.filterModel != null && filterRequset.filterModel.Count() != 0)
                {
                    foreach (var filter in filterRequset.filterModel)
                    {
                        var filterKey = "";

                        if (filter.Key.IndexOf(".") != -1)
                        {
                            filterKey = filter.Key.Substring(filter.Key.IndexOf(".") + 1).ToString();
                            var filterForignTable = filter.Key.Substring(0, filter.Key.IndexOf(".")).ToString();

                            var ForginTable = Expression.Property(parameter, filterForignTable);
                            var sortTableCol = Expression.Property(ForginTable, filterKey);

                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(sortTableCol, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);

                            var ForginTableInclude = Expression.Lambda(ForginTable, parameter);
                            lambda = Expression.Lambda<Func<EmployeeDTO, bool>>(condition, parameter);
                            if (combinedCondition == null)
                            {
                                combinedCondition = lambda;
                            }
                            else
                            {
                                combinedCondition = CombineLambdas(combinedCondition, lambda, filterRequset);

                            }
                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            if (filter.Value.filterType == "date")
                            {
                                var date = DateOnly.FromDateTime(DateTime.Parse(filter.Value.dateFrom));
                                var value = Expression.Constant(date);
                                var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                                lambda = Expression.Lambda<Func<EmployeeDTO, bool>>(condition, parameter);
                            }
                            else
                            {

                                var value = Expression.Constant(filter.Value.filter);
                                var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                                lambda = Expression.Lambda<Func<EmployeeDTO, bool>>(condition, parameter);
                            }
                            if (combinedCondition == null)
                            {
                                combinedCondition = lambda;
                            }
                            else
                            {
                                combinedCondition = CombineLambdas(combinedCondition, lambda, filterRequset);


                            }
                        }
                        if (combinedCondition == null)
                        {
                            combinedCondition = lambda;
                        }
                        else
                        {
                            combinedCondition = CombineLambdas(combinedCondition, lambda, filterRequset);


                        }
                    }


                    query = query.Where(combinedCondition);

                }

                if (filterRequset.sortModel.colId.IndexOf(".") != -1)
                {
                    // for Make Key and Table subString
                    var SortKey = filterRequset.sortModel.colId.Substring(filterRequset.sortModel.colId.IndexOf(".") + 1).ToString();
                    var ForignKeyTable = filterRequset.sortModel.colId.Substring(0, filterRequset.sortModel.colId.IndexOf(".")).ToString();

                    //make exprssion
                    var ForignKeyTableColumn = Expression.Property(parameter, ForignKeyTable);

                    var sortTableCol = Expression.Property(ForignKeyTableColumn, SortKey);

                    var ForginTableInclude = Expression.Lambda(sortTableCol, parameter);

                    var sortCodition = Expression.Lambda<Func<EmployeeDTO, string>>(sortTableCol, parameter);

                    switch (filterRequset.sortModel.sortOrder)
                    {
                        case "asc":
                            query = query.OrderBy(sortCodition);

                            break;
                        case "desc":
                            query = query.OrderByDescending(sortCodition);

                            break;
                    };
                }
                else
                {
                    if (filterRequset.sortModel != null)
                    {
                        var SortColumn = Expression.Property(parameter, filterRequset.sortModel.colId);
                        var sortCodition = Expression.Lambda<Func<EmployeeDTO, string>>(SortColumn, parameter);
                        switch (filterRequset.sortModel.sortOrder)
                        {
                            case "asc":
                                query = query.OrderBy(sortCodition);
                                break;
                            case "desc":
                                query = query.OrderByDescending(sortCodition);
                                break;
                        };
                    }
                    else
                    {

                        query = query;
                    }
                }
                var totalRecord = query.ToList().Count();
                int skip = (int)((filterRequset.PageNumber - 1) * filterRequset.PageSize);
                //int last =
                var employee = query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToList();

                ResponseEmployeeDTO Response = new ResponseEmployeeDTO()
                {
                    Employee = employee,
                    TotalRecord = totalRecord
                };

                if (employee == null)
                {
                    response.Message = "No Any Desigantion";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Desigantion Get Sucesfully";
                    response.HttpResponse = Response;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }


                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

    


