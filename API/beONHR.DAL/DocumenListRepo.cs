using beONHR.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using beONHR.Entities;
using beONHR.Entities.Context;
using System.Collections.Generic;
using System.Linq.Expressions;
using beONHR.Entities.DTO.Enum;
using System.Security.Principal;


namespace beONHR.DAL
{
    public interface IDocumenListRepo
    {
        Task<ClientResponse> SaveDocumentList(DocumentListDTO input);
        Task<ClientResponse> GetDocumentList();
        Task<ClientResponse> GetFilterDocumentList(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteDocumentList(Guid id);

        Task<ClientResponse> GetDocumentListById(Guid id);

        Task<ClientResponse> GetDocumentListByEmployeeIdOrEntityId(Guid? employeeId = null, Guid? entityId = null,string ? fileName = null);
    }
    public class DocumentListRepo : IDocumenListRepo
    {
        private readonly MainContext _context;

        public DocumentListRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveDocumentList(DocumentListDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input == null)
                {
                    response.Message = "Document data is null";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }

                // Check if the asset with the given ID exists
                var documentlist = await _context.DocumentList.FindAsync(input.Id);

                if (documentlist == null)
                {

                    DocumentList newdocumentList = new DocumentList
                    {
                        TabName = input.TabName,
                        Modulename = input.Modulename,
                        FileName = input.FileName,
                        Documents = input.Documents,
                        EmployeeId= input.EmployeeId,

                    };

                    _context.DocumentList.Add(newdocumentList);
                    await _context.SaveChangesAsync();

                    response.Message = "Document saved successfully";
                    response.HttpResponse = newdocumentList.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    // Update existing asset
                    documentlist.TabName = input.TabName;
                    documentlist.Modulename = input.Modulename;
                    documentlist.FileName = input.FileName;
                    documentlist.Documents = input.Documents;
                    documentlist.EmployeeId = input.EmployeeId;

                    _context.DocumentList.Update(documentlist);
                    await _context.SaveChangesAsync();

                    response.Message = "Document updated successfully";
                    response.HttpResponse = documentlist.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetDocumentList()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var documentlist = await _context.DocumentList.Where(x => x.IsDeleted != true)
                    .Select(e => new DocumentListDTO {
                        Id=e.Id,
                        TabName=e.TabName,
                        Modulename=e.Modulename,
                        FileName=e.FileName,
                        //Documents = e.Documents,
                        EmployeeId=e.EmployeeId,
                    }).ToListAsync();

                response.Message = "Document retrieved successfully";
                response.HttpResponse = documentlist;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<ClientResponse> DeleteDocumentList(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var documentlist = await _context.DocumentList.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (documentlist != null)
                {
                    documentlist.IsDeleted = true;


                    _context.DocumentList.Update(documentlist);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Document Deleted Failed";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Document Deleted Successfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Document not Exists";
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

        public async Task<ClientResponse> GetDocumentListById(Guid Id)
        {
            ClientResponse response = new();
            try
            {
                // Get the asset by ID
                var documentlist = await _context.DocumentList.FindAsync(Id);

                if (documentlist == null)
                {
                    response.Message = "Document not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Document retrieved successfully";
                    response.HttpResponse = documentlist;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public async Task<ClientResponse> GetDocumentListByEmployeeId(Guid Id)
        //{
        //    ClientResponse response = new();
        //    try
        //    {
        //        // Get the asset by ID
        //        var documentlist = await _context.DocumentList.Where(x => x.EmployeeId == Id && x.IsDeleted != true).FirstOrDefaultAsync();

        //        if (documentlist != null)
        //        {

        //            response.Message = "documentlist Get Sucesfully";
        //            response.HttpResponse = documentlist;
        //            response.IsSuccess = true;
        //            response.StatusCode = HttpStatusCode.OK;


        //        }
        //        else
        //        {
        //            response.Message = "documentlist not Exists";
        //            response.StatusCode = HttpStatusCode.NoContent;
        //            response.IsSuccess = false;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public async Task<ClientResponse> GetDocumentListByEmployeeIdOrEntityId(Guid? employeeId = null, Guid? entityId = null, string? fileName = null)
{
    ClientResponse response = new();
    try
    {
        List<DocumentListDTO> documentEntries = new List<DocumentListDTO>();

        if (employeeId.HasValue)
        {
            // Fetch related Education entity and create document entries
            var educations = await _context.Educations
                .Where(e => e.Employee == employeeId.Value && !e.IsDeleted)
                .ToListAsync();
            foreach (var education in educations)
            {
                documentEntries.Add(new DocumentListDTO
                {
                    Id = education.Id,
                    TabName = "Education",
                    Modulename = "Education",
                    FileName = "Certificate",
                    Documents = null,
                    EmployeeId = education.Employee
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = education.Id,
                    TabName = "Education",
                    Modulename = "Education",
                    FileName = "Anabin",
                    Documents = null,
                    EmployeeId = education.Employee
                });
            }

            // Fetch related JobHistory entity and create document entries
            var jobHistories = await _context.JobHistories
                .Where(jh => jh.EmployeeId == employeeId.Value && !jh.IsDeleted)
                .ToListAsync();
            foreach (var jobHistory in jobHistories)
            {
                documentEntries.Add(new DocumentListDTO
                {
                    Id = jobHistory.Id,
                    TabName = "JobHistory",
                    Modulename = "JobHistory",
                    FileName = "Document",
                    Documents = null,
                    EmployeeId = jobHistory.EmployeeId
                });
            }

            // Fetch related IdentityCard entity and create document entries
            var identityCards = await _context.IdentityCards
                .Include(ic => ic.WorkPermitDetails)
                .Where(ic => ic.EmployeeId == employeeId.Value && !ic.IsDeleted)
                .ToListAsync();
            foreach (var identityCard in identityCards)
            {
                documentEntries.Add(new DocumentListDTO
                {
                    Id = identityCard.Id,
                    TabName = "IdentityCard",
                    Modulename = "IdentityCard",
                    FileName = "Passport",
                    Documents = null,
                    EmployeeId = identityCard.EmployeeId
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = identityCard.Id,
                    TabName = "IdentityCard",
                    Modulename = "IdentityCard",
                    FileName = "Visa",
                    Documents = null,
                    EmployeeId = identityCard.EmployeeId
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = identityCard.Id,
                    TabName = "IdentityCard",
                    Modulename = "IdentityCard",
                    FileName = "Blue Card",
                    Documents = null,
                    EmployeeId = identityCard.EmployeeId
                });

                foreach (var workPermit in identityCard.WorkPermitDetails)
                {
                    documentEntries.Add(new DocumentListDTO
                    {
                        Id = workPermit.Id,
                        TabName = "IdentityCard",
                        Modulename = "IdentityCard",
                        FileName = "WorkPermit_" + workPermit.PermitType,
                        Documents = null,
                        EmployeeId = identityCard.EmployeeId
                    });
                }
            }

            // Fetch related Employee entity and create document entries
            var personal = await _context.Employees
                .Include(ie => ie.LanguageCompetences)
                .FirstOrDefaultAsync(ie => ie.Id == employeeId.Value && !ie.IsDeleted);
            if (personal != null)
            {
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "General",
                    FileName = "Profile Photo",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "General",
                    FileName = "Personal Sheet",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "General",
                    FileName = "Social Security",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Child info",
                    FileName = "Socialcare Insurance",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Child info",
                    FileName = "Birth Certificate",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Probation",
                    FileName = "Adjusted",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Termination",
                    FileName = "Termination Doc",
                    Documents = null,
                    EmployeeId = employeeId.Value
                });

                foreach (var languageCompetence in personal.LanguageCompetences)
                {
                    documentEntries.Add(new DocumentListDTO
                    {
                        Id = languageCompetence.Id,
                        TabName = "Personal",
                        Modulename = "Language",
                        FileName = "Language Certificate",
                        Documents = null,
                        EmployeeId = employeeId.Value
                    });
                }
            }
        }

        if (entityId.HasValue)
        {
            // Fetch Education entity by Id
            var education = await _context.Educations
                .FirstOrDefaultAsync(e => e.Id == entityId.Value && !e.IsDeleted);
            if (education != null)
                    {
                        if (string.IsNullOrEmpty(fileName) || fileName == "Certificate")
                        {
                            documentEntries.Add(new DocumentListDTO
                            {
                                Id = education.Id,
                                TabName = "Education",
                                Modulename = "Education",
                                FileName = "Certificate",
                                Documents = education.Certificate,
                                EmployeeId = education.Employee
                            });
                        }
                        if (string.IsNullOrEmpty(fileName) || fileName == "Anabin")
                        {
                            documentEntries.Add(new DocumentListDTO
                            {
                                Id = education.Id,
                                TabName = "Education",
                                Modulename = "Education",
                                FileName = "Anabin",
                                Documents = education.Anabin,
                                EmployeeId = education.Employee
                            });
                        }
            }

                    // Fetch JobHistory entity by Id
                    var jobHistory = await _context.JobHistories
                .FirstOrDefaultAsync(jh => jh.Id == entityId.Value && !jh.IsDeleted);
                    if (jobHistory != null)
                    {
                        if (string.IsNullOrEmpty(fileName) || fileName == "Document")
                        {
                            documentEntries.Add(new DocumentListDTO
                            {
                                Id = jobHistory.Id,
                                TabName = "JobHistory",
                                Modulename = "JobHistory",
                                FileName = "Document",
                                Documents = jobHistory.Document,
                                EmployeeId = jobHistory.EmployeeId
                            });
                        }
                    }

                    // Fetch IdentityCard entity by Id
                    var identityCard = await _context.IdentityCards
                .Include(ic => ic.WorkPermitDetails)
                .FirstOrDefaultAsync(ic => ic.Id == entityId.Value && !ic.IsDeleted);
                    if (identityCard != null)
                    {
                        if (string.IsNullOrEmpty(fileName) || fileName == "Passport")
                        {
                            documentEntries.Add(new DocumentListDTO
                            {
                                Id = identityCard.Id,
                                TabName = "IdentityCard",
                                Modulename = "IdentityCard",
                                FileName = "Passport",
                                Documents = identityCard.Passport,
                                EmployeeId = identityCard.EmployeeId
                            });
                        }
                        if (string.IsNullOrEmpty(fileName) || fileName == "Visa")
                        {
                            documentEntries.Add(new DocumentListDTO
                            {
                                Id = identityCard.Id,
                                TabName = "IdentityCard",
                                Modulename = "IdentityCard",
                                FileName = "Visa",
                                Documents = identityCard.Visa,
                                EmployeeId = identityCard.EmployeeId
                            });
                        }
                        if (string.IsNullOrEmpty(fileName) || fileName == "Blue Card")
                        {
                            documentEntries.Add(new DocumentListDTO
                            {
                                Id = identityCard.Id,
                                TabName = "IdentityCard",
                                Modulename = "IdentityCard",
                                FileName = "Blue Card",
                                Documents = identityCard.BlueCard,
                                EmployeeId = identityCard.EmployeeId
                            });
                        }

                        foreach (var workPermit in identityCard.WorkPermitDetails)
                        {
                            if (string.IsNullOrEmpty(fileName) || fileName == "WorkPermit_" + workPermit.PermitType)
                            {
                                documentEntries.Add(new DocumentListDTO
                                {
                                    Id = workPermit.Id,
                                    TabName = "IdentityCard",
                                    Modulename = "IdentityCard",
                                    FileName = "WorkPermit_" + workPermit.PermitType,
                                    Documents = workPermit.Document,
                                    EmployeeId = identityCard.EmployeeId
                                });
                            }
                        }
                    }

                    // Fetch Employee entity by Id
                    var personal = await _context.Employees
                .Include(ie => ie.LanguageCompetences)
                .FirstOrDefaultAsync(ie => ie.Id == entityId.Value && !ie.IsDeleted);
            if (personal != null)
            {
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "General",
                    FileName = "Profile Photo",
                    Documents = personal.ProfilePhoto,
                    EmployeeId = personal.Id
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "General",
                    FileName = "Personal Sheet",
                    Documents = personal.PersonalSheet,
                    EmployeeId = personal.Id
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "General",
                    FileName = "Social Security",
                    Documents = personal.SocialSecurityFile,
                    EmployeeId = personal.Id
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Child info",
                    FileName = "Socialcare Insurance",
                    Documents = personal.socialcareinsurance,
                    EmployeeId = personal.Id
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Child info",
                    FileName = "Birth Certificate",
                    Documents = personal.BirthCertificate,
                    EmployeeId = personal.Id
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Probation",
                    FileName = "Adjusted",
                    Documents = personal.AdjustedDocument,
                    EmployeeId = personal.Id
                });
                documentEntries.Add(new DocumentListDTO
                {
                    Id = personal.Id,
                    TabName = "Personal",
                    Modulename = "Termination",
                    FileName = "Termination Doc",
                    Documents = personal.Terminationofemplyee,
                    EmployeeId = personal.Id
                });

                foreach (var languageCompetence in personal.LanguageCompetences)
                {
                    documentEntries.Add(new DocumentListDTO
                    {
                        Id = languageCompetence.Id,
                        TabName = "Personal",
                        Modulename = "Language",
                        FileName = "Language Certificate",
                        Documents = languageCompetence.LanguagesCertificate,
                        EmployeeId = personal.Id
                    });
                }
            }
        }

        if (documentEntries.Any())
        {
            response.Message = "Documents retrieved successfully";
            response.HttpResponse = documentEntries;
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
        }
        else
        {
            response.Message = "Document list not found";
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



        static Expression<Func<DocumentList, bool>> CombineLambdas(Expression<Func<DocumentList, bool>> expr1, Expression<Func<DocumentList, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(DocumentList));
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

            return Expression.Lambda<Func<DocumentList, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterDocumentList(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.DocumentList.Where(x => x.IsDeleted != true).Include(x => x.Employee).AsQueryable();
                // Loop through each filter

                Expression<Func<DocumentList, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(DocumentList));
                Expression<Func<DocumentList, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<DocumentList, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<DocumentList, bool>>(condition, parameter);
                            //query = query.Where(lambda);
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

                    var sortCodition = Expression.Lambda<Func<DocumentList, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<DocumentList, string>>(SortColumn, parameter);
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
                var totalRecord = query.Count();
                int skip = (int)((filterRequset.PageNumber - 1) * filterRequset.PageSize);
                //int last =
                var documentlist = await query
                            .Skip(skip).Include(e=>e.Employee)
                            .Take((int)(filterRequset.PageSize)).Select(e => new DocumentListDTO
                            {
                                Id = e.Id,
                                TabName=e.TabName,
                                Modulename=e.Modulename,
                                FileName=e.FileName,
                                //Documents=e.Documents,
                                EmployeeId=e.EmployeeId,

                            }).ToListAsync();

                ResponseDocumentListDto Response = new ResponseDocumentListDto()
                {
                    documentlists = documentlist,
                    TotalRecord = totalRecord
                };

                if (documentlist == null)
                {
                    response.Message = "No Any Document";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Documents Get Successfully";
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

        // *********************** filter Method End  *********************************//
    }
}
