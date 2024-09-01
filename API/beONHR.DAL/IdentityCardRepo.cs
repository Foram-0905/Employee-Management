using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IIdentityRepo
    {
        Task<ClientResponse> SaveIdentity(IdentityCardDTO input);
        Task<ClientResponse> GetIdentity();
        Task<ClientResponse> DeleteIdentity(Guid id);
        Task<ClientResponse> GetIdentityById(Guid id);
        Task<ClientResponse> GetIdentityByEmployeeId(Guid id);
    }

    public class IdentityRepo : IIdentityRepo
    {
        private readonly MainContext _context;

        public IdentityRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveIdentity(IdentityCardDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {

                    var identitycards = await _context.IdentityCards
                        .FirstOrDefaultAsync(x => x.Id == input.Id && x.Passport == input.Passport && x.EmployeeId == input.EmployeeId);
                    if (identitycards == null)
                    {
                        IdentityCard model = new()
                        {
                            Id = input.Id, // Assuming you generate a new GUID for each new employee

                            Passport = input.Passport,
                            Visa = input.Visa,
                            VisaStartdate = input.VisaStartdate,
                            EmployeeId = input.EmployeeId,
                            VisaExpirytdate = input.VisaExpirytdate,
                            BlueCard = input.BlueCard,
                            BlueCardStartdate = input.BlueCardStartdate,
                            BlueCardExpirytdate = input.BlueCardExpirytdate,


                            //LanguageCompetences = input.LanguageCompetences
                            WorkPermitDetails = input.WorkPermitDetails.Select(c => new WorkPermitDetail
                            {
                                Id = Guid.NewGuid(),
                                PermitType = c.PermitType,
                                PermitStartdate = c.PermitStartdate,
                                PermitExpirytdate = c.PermitExpirytdate,
                                Document = c.Document,

                            }).ToList()
                        };



                        await _context.IdentityCards.AddAsync(model);

                        //DocumentList passportDocument = new DocumentList
                        //{
                        //    Id = Guid.NewGuid(),
                        //    TabName = "Identity",
                        //    Modulename = "Identity",
                        //    FileName = "PassportDocument",
                        //    Documents = input.Passport,
                        //    EmployeeId = input.EmployeeId,
                        //};
                        //_context.DocumentList.Add(passportDocument);

                        //DocumentList visaDocument = new DocumentList
                        //{
                        //    Id = Guid.NewGuid(),
                        //    TabName = "Identity",
                        //    Modulename = "Identity",
                        //    FileName = "VisaDocument",
                        //    Documents = input.Visa,
                        //    EmployeeId = input.EmployeeId,
                        //};
                        //_context.DocumentList.Add(visaDocument);

                        //DocumentList blueCardDocument = new DocumentList
                        //{
                        //    Id = Guid.NewGuid(),
                        //    TabName = "Identity",
                        //    Modulename = "Identity",
                        //    FileName = "BlueCardDocument",
                        //    Documents = input.BlueCard,
                        //    EmployeeId = input.EmployeeId,
                        //};
                        //_context.DocumentList.Add(blueCardDocument);

                        //foreach (var workPermitDetail in input.WorkPermitDetails)
                        //{
                        //    DocumentList workPermitDocument = new DocumentList
                        //    {
                        //        Id = Guid.NewGuid(),
                        //        TabName = "Identity",
                        //        Modulename = "Identity",
                        //        FileName = "WorkPermitDocument",
                        //        Documents = workPermitDetail.Document,
                        //        EmployeeId = input.EmployeeId,
                        //    };
                        //    _context.DocumentList.Add(workPermitDocument);
                        //}

                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "IdentityCards not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "IdentityCards inserted successfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "IdentityCards already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }

                    return response;
                }

                else
                {
                    var identity = await _context.IdentityCards.Include(x => x.WorkPermitDetails)
                        .FirstOrDefaultAsync(x => x.Id == input.Id && x.IsDeleted == false);

                    if (identity != null)
                    {
                        identity.Passport=input.Passport;
                        identity.Visa = input.Visa;
                        identity.VisaStartdate = input.VisaStartdate;
                        identity.VisaExpirytdate = input.VisaExpirytdate;
                        identity.BlueCard = input.BlueCard;
                        identity.BlueCardStartdate = input.BlueCardStartdate;
                        identity.BlueCardExpirytdate = input.BlueCardExpirytdate;
                        if (identity.WorkPermitDetails != null && identity.WorkPermitDetails.Count() != 0)
                        {
                            _context.WorkPermitDetail.RemoveRange(identity.WorkPermitDetails);
                        }
                        identity.WorkPermitDetails = input.WorkPermitDetails.Select(c => new WorkPermitDetail{
                            Id= c.Id,
                            PermitType= c.PermitType,
                            PermitStartdate= c.PermitStartdate,
                            PermitExpirytdate= c.PermitExpirytdate,
                            Document = c.Document
                        }).ToList();

                        _context.IdentityCards.Update(identity);

                  //      var passportDocument = await _context.DocumentList
                  //.FirstOrDefaultAsync(d => d.FileName == "PassportDocument" && d.EmployeeId == input.EmployeeId);
                  //      if (passportDocument != null)
                  //      {
                  //          passportDocument.Documents = input.Passport;
                  //          _context.DocumentList.Update(passportDocument);
                  //      }
                  //      else
                  //      {
                  //          DocumentList newPassportDocument = new DocumentList
                  //          {
                  //              Id =input.Id,
                  //              TabName = "Identity",
                  //              Modulename = "Identity",
                  //              FileName = "PassportDocument",
                  //              Documents = input.Passport,
                  //              EmployeeId = input.EmployeeId,
                  //          };
                  //          _context.DocumentList.Add(newPassportDocument);
                  //      }

                  //      var visaDocument = await _context.DocumentList
                  //          .FirstOrDefaultAsync(d => d.FileName == "VisaDocument" && d.EmployeeId == input.EmployeeId);
                  //      if (visaDocument != null)
                  //      {
                  //          visaDocument.Documents = input.Visa;
                  //          _context.DocumentList.Update(visaDocument);
                  //      }
                  //      else
                  //      {
                  //          DocumentList newVisaDocument = new DocumentList
                  //          {
                  //              Id = input.Id,
                  //              TabName = "Identity",
                  //              Modulename = "Identity",
                  //              FileName = "VisaDocument",
                  //              Documents = input.Visa,
                  //              EmployeeId = input.EmployeeId,
                  //          };
                  //          _context.DocumentList.Add(newVisaDocument);
                  //      }

                  //      var blueCardDocument = await _context.DocumentList
                  //          .FirstOrDefaultAsync(d => d.FileName == "BlueCardDocument" && d.EmployeeId == input.EmployeeId);
                  //      if (blueCardDocument != null)
                  //      {
                  //          blueCardDocument.Documents = input.BlueCard;
                  //          _context.DocumentList.Update(blueCardDocument);
                  //      }
                  //      else
                  //      {
                  //          DocumentList newBlueCardDocument = new DocumentList
                  //          {
                  //              Id = input.Id,
                  //              TabName = "Identity",
                  //              Modulename = "Identity",
                  //              FileName = "BlueCardDocument",
                  //              Documents = input.BlueCard,
                  //              EmployeeId = input.EmployeeId,
                  //          };
                  //          _context.DocumentList.Add(newBlueCardDocument);
                  //      }

                  //      foreach (var workPermitDetail in input.WorkPermitDetails)
                  //      {
                  //          var workPermitDocument = await _context.DocumentList
                  //              .FirstOrDefaultAsync(d => d.FileName == "WorkPermitDocument" && d.EmployeeId == input.EmployeeId);
                  //          if (workPermitDocument != null)
                  //          {
                  //              workPermitDocument.Documents = workPermitDetail.Document;
                  //              _context.DocumentList.Update(workPermitDocument);
                  //          }
                  //          else
                  //          {
                  //              DocumentList newWorkPermitDocument = new DocumentList
                  //              {
                  //                  Id = input.Id,
                  //                  TabName = "Identity",
                  //                  Modulename = "Identity",
                  //                  FileName = "WorkPermitDocument",
                  //                  Documents = workPermitDetail.Document,
                  //                  EmployeeId = input.EmployeeId,
                  //              };
                  //              _context.DocumentList.Add(newWorkPermitDocument);
                  //          }
                  //      }

                        var resp = await _context.SaveChangesAsync();
                        if (resp == 0)
                        {
                            response.Message = "identity not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "identity updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;

                        }
                    }
                    else
                    {
                        response.Message = "Identity Does not Exist";
                        response.StatusCode = HttpStatusCode.NoContent;
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
                // Log or handle the exception as needed
                throw ex;
            }
        }
        public async Task<ClientResponse> GetIdentity()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var identity = await _context.IdentityCards
                    .Include(x => x.Employee)
                    .Include(x => x.WorkPermitDetails)// Include Work Permit Details
                    .Where(x => x.IsDeleted != true)
                    .Select(x => new IdentityCardDTO
                    {
                        Id = x.Id,
                        Visa = x.Visa,
                        VisaStartdate = x.VisaStartdate,
                        VisaExpirytdate = x.VisaExpirytdate,
                        BlueCard = x.BlueCard,
                        EmployeeId = x.EmployeeId,
                        
                        BlueCardStartdate = x.BlueCardStartdate,
                        BlueCardExpirytdate = x.BlueCardExpirytdate,
                        Passport = x.Passport,

                        WorkPermitDetails = x.WorkPermitDetails.Select(c => new WorkPermitDetailDTO
                        {
                            Id = c.Id,
                            PermitType = c.PermitType,
                            PermitStartdate = c.PermitStartdate,
                            PermitExpirytdate = c.PermitExpirytdate,
                            Document = c.Document,
                        }).ToList(),
                    }).ToListAsync();

                if (identity == null || identity.Count == 0)
                {
                    response.Message = "No Any identity";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                }
                else
                {
                    response.Message = "identity Get Sucesfully";
                    response.HttpResponse = identity;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteIdentity(Guid id)
        {
            ClientResponse response = new();
            try
            {
                var identity = await _context.IdentityCards
                    .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted != true);

                if (identity != null)
                {
                    identity.IsDeleted = true;
                    _context.IdentityCards.Update(identity);
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "IdentityCards Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "IdentityCards Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "IdentityCards not Exists";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientResponse> GetIdentityById(Guid id)
        {
            ClientResponse response = new();
            try
            {
                var identity = await _context.IdentityCards
                    .Where(x => x.Id == id && x.IsDeleted != true)
                    .Include(x => x.Employee)
                    .Include(x => x.WorkPermitDetails)
                    .Select(x => new IdentityCardDTO
                    {
                        Id = x.Id,
                        Visa = x.Visa,
                        VisaStartdate = x.VisaStartdate,
                        VisaExpirytdate = x.VisaExpirytdate,
                        BlueCard = x.BlueCard,
                        EmployeeId = x.EmployeeId,

                        BlueCardStartdate = x.BlueCardStartdate,
                        BlueCardExpirytdate = x.BlueCardExpirytdate,
                        Passport = x.Passport,

                        WorkPermitDetails = x.WorkPermitDetails.Select(c => new WorkPermitDetailDTO
                        {
                            Id = c.Id,
                            PermitType = c.PermitType,
                            PermitStartdate = c.PermitStartdate,
                            PermitExpirytdate = c.PermitExpirytdate,
                            Document = c.Document,
                        }).ToList(),
                    }).FirstOrDefaultAsync();
                // Include Work Permit Details

                if (identity != null)
                {
                    response.Message = "Identity Get Sucesfully";
                    response.HttpResponse = identity;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Identity not Exists";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientResponse> GetIdentityByEmployeeId(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var identity = await _context.IdentityCards.Where(x => x.EmployeeId == id && x.IsDeleted != true).Select(x => new IdentityCardDTO
                {
                    Id = x.Id,
                    Visa = x.Visa,
                    VisaStartdate = x.VisaStartdate,
                    VisaExpirytdate = x.VisaExpirytdate,
                    BlueCard = x.BlueCard,
                    EmployeeId = x.EmployeeId,

                    BlueCardStartdate = x.BlueCardStartdate,
                    BlueCardExpirytdate = x.BlueCardExpirytdate,
                    Passport = x.Passport,

                    WorkPermitDetails = x.WorkPermitDetails.Select(c => new WorkPermitDetailDTO
                    {
                        Id = c.Id,
                        PermitType = c.PermitType,
                        PermitStartdate = c.PermitStartdate,
                        PermitExpirytdate = c.PermitExpirytdate,
                        Document = c.Document,
                    }).ToList(),
                }).FirstOrDefaultAsync();

                if (identity != null)
                {

                    response.Message = "Identity Get Sucesfully";
                    response.HttpResponse = identity;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Identity not Exists";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
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