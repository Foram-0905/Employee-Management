using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities;
using beONHR.Entities.Context;
using System.Diagnostics;
using System.Net;
using Microsoft.EntityFrameworkCore;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Crypto;
using Azure;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace beONHR.DAL
{
    public interface IDesignationRepo
    {
        Task<ClientResponse> SaveDesignation(DesignationDto input);
        Task<ClientResponse> GetDesignation();
        Task<ClientResponse> GetFilterDesignation(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteDesignation(Guid id);
        Task<ClientResponse> GetDesignationById(Guid id);
    }

    public class DesignationRepo : IDesignationRepo
    {
        private readonly MainContext _context;
        public DesignationRepo(MainContext context)
        {
            _context = context;
        }
        public async Task<ClientResponse> SaveDesignation(DesignationDto input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var designation = await _context.ManageDesignations.Where(x => x.InitialStatus == input.InitialStatus && x.Designation == input.Designation && x.IsDeleted != true).FirstOrDefaultAsync();
                    //3fa85f64 - 5717 - 4562 - b3fc - 2c963f66afa6
                    if (designation == null)
                    {
                        ManageDesignation model = new ManageDesignation
                        {
                            InitialStatus = input.InitialStatus,
                            Designation = input.Designation,
                            DisplaySequence = input.DisplaySequence,
                            ShortWord = input.ShortWord,
                        };

                        await _context.ManageDesignations.AddAsync(model);
                        var res = await _context.SaveChangesAsync();


                        if (res == 0)
                        {
                            response.Message = "Desigantion not insert";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Desigantion insert Sucesfully";
                            response.HttpResponse = model.Id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Desigantion already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var designation = await _context.ManageDesignations.Where(x => x.Id == input.Id && x.IsDeleted == false).FirstOrDefaultAsync();

                    if (designation != null)
                    {
                        designation.Id = input.Id;
                        designation.InitialStatus = input.InitialStatus;
                        designation.Designation = input.Designation;
                        designation.ShortWord = input.ShortWord;
                        designation.DisplaySequence = input.DisplaySequence;


                        _context.ManageDesignations.Update(designation);
                        var res = await _context.SaveChangesAsync();


                        if (res == 0)
                        {
                            response.Message = "Desigantion not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Desigantion updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Desigantion Not Exists";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    return response;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetDesignation()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var desigantion = await _context.ManageDesignations.Include(x => x.SLGGroup).Where(x => x.IsDeleted != true).OrderBy(x => x.Designation).ToListAsync();

                if (desigantion == null)
                {
                    response.Message = "No Any Desigantion";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                response.Message = "Desigantion Get Sucesfully";
                response.HttpResponse = desigantion;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }



        // *********************** Filter Method Start   *********************************//



        static Expression<Func<ManageDesignation, bool>> CombineLambdas(Expression<Func<ManageDesignation, bool>> expr1, Expression<Func<ManageDesignation, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(ManageDesignation));
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

            return Expression.Lambda<Func<ManageDesignation, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterDesignation(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.ManageDesignations.Where(x => x.IsDeleted != true).Include(x => x.SLGGroup).AsQueryable();
                // Loop through each filter

                Expression<Func<ManageDesignation, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(ManageDesignation));
                Expression<Func<ManageDesignation, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<ManageDesignation, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<ManageDesignation, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<ManageDesignation, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<ManageDesignation, string>>(SortColumn, parameter);
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
                var desigantion = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseDesignationDto Response = new ResponseDesignationDto()
                {
                    designation = desigantion,
                    TotalRecord = totalRecord
                };

                if (desigantion == null)
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





        // *********************** filter Method End  *********************************//






        public async Task<ClientResponse> DeleteDesignation(Guid id)
        {

            ClientResponse response = new();
            try
            {
                var designation = await _context.ManageDesignations.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (designation != null)
                {
                    designation.IsDeleted = true;
                    _context.ManageDesignations.Update(designation);
                    var res = await _context.SaveChangesAsync();


                    if (res == 0)
                    {
                        response.Message = "Desigantion Deleted Faild";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Desigantion Deleted Sucesfully";
                        response.HttpResponse = null;
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    response.Message = "Desigantion not Exists";
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

        public async Task<ClientResponse> GetDesignationById(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var designation = await _context.ManageDesignations.Where(x => x.Id == id && x.IsDeleted != true).FirstOrDefaultAsync();
                
                if (designation != null)
                {

                    response.Message = "Desigantion Get Sucesfully";
                    response.HttpResponse = designation;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "Desigantion not Exists";
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
