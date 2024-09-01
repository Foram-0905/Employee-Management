using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using beONHR.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using System.Data; 
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq.Expressions;
using System.Diagnostics.Eventing.Reader;
using beONHR.Entities.Context;

namespace beONHR.DAL
{
    public interface IRoleRepo
    {
        Task<ClientResponse> SaveRole(RoleDTO input);
        Task<ClientResponse> GetRole();
        Task<ClientResponse> GetRoleById(Guid id);
        Task<ClientResponse> GetFilterRole(FilterRequsetDTO filterRequset);
        Task<ClientResponse>DeleteRole(string id);
       
    }

    public class RoleRepo : IRoleRepo
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly RoleManager<AspNetRoles> _roleManager; 
        private readonly MainContext _context;

      
        public RoleRepo(Microsoft.AspNetCore.Identity.UserManager<AspNetUsers> userManager, RoleManager<AspNetRoles> roleManager,MainContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<ClientResponse> SaveRole(RoleDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var Role = await _roleManager.FindByIdAsync(input.Id);

                    if (Role is null)
                    {
                        AspNetRoles model = new AspNetRoles
                        {
                            Name = input.Name,
                            Description = input.Description,
                        };

                        var roleExistsYet = _roleManager.RoleExistsAsync(model.Name);
                        if (roleExistsYet.Result)
                        {
                            response.Message = "Role already exist";
                            response.StatusCode = HttpStatusCode.BadRequest;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            var roleResult = await _roleManager.CreateAsync(model);

                            if (roleResult == IdentityResult.Success)
                            {
                                response.Message = "Role Add Sucesfully";
                                response.StatusCode = HttpStatusCode.NoContent;
                                response.IsSuccess = true;

                            }
                            else
                            {
                                response.Message = "Role Not Add";
                                response.HttpResponse = model.Id;
                                response.IsSuccess =  false;
                                response.StatusCode = HttpStatusCode.OK;
                            }
                        }                                          
                    }
                    else if (Role.IsDeleted != true)
                    {
                        response.Message = "Role already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var Role = await _roleManager.FindByIdAsync(input.Id);

                    if (Role.Id != null)
                    {

                        Role.Name = input.Name;
                        Role.Description = input.Description;

                        var result = await _roleManager.UpdateAsync(Role);

                        if (result != IdentityResult.Success)
                        {
                            response.Message = "Role not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "Role updated Sucesfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "Role Not Exists";
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


        public async Task<ClientResponse> GetRole()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var roles = await _roleManager.Roles.Where(x => x.IsDeleted != true && x.Name != "SysAdmin" && x.Name != "beOnAdmin").OrderBy(x => x.Name).ToListAsync();

                //var Roles = new List<AspNetRoles>();

                //foreach (var role in roles)
                //{
                //    // Example condition: Check if the role name contains 'Admin'
                //    if (role.IsDeleted != true)
                //    {
                //        if(role.Name != "beOnAdmin" && role.Name!= "SysAdmin")
                //        {

                //        Roles.Add(role);
                //        }
                //    }
                //}

                if (roles == null)
                {
                    response.Message = "Role Get Sucesfully";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                response.Message = "No Any Role";
                response.HttpResponse = roles;
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;

                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ClientResponse> GetRoleById(Guid id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var roles = await _roleManager.FindByIdAsync(id.ToString());

                if (roles == null)
                {
                    response.Message = "No Any Role";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Role Get Sucesfully";
                    response.HttpResponse = roles;
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
        public async Task<ClientResponse> DeleteRole(string id)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                // Find the role by its ID
                var role = await _roleManager.FindByIdAsync(id.ToString());

                if (role == null)
                {
                    response.Message = "Role not found";
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    // Check if any employees are assigned to this role
                    var employeesWithRole = await _context.Employees
                                                          .Where(e => e.RoleId == id)
                                                          .ToListAsync();

                    if (employeesWithRole.Any())
                    {
                        response.Message = $"Role '{role.Name}'  cannot be deleted because it is assigned to one or more employees.";
                        response.IsSuccess = false;
                        response.StatusCode = HttpStatusCode.OK;
                      
                    }
                    else
                    {
                        // Soft delete the role by updating the IsDeleted property
                        role.IsDeleted = true;

                        // Update the role in the database
                        var result = await _roleManager.UpdateAsync(role);

                        if (result.Succeeded)
                        {
                            response.Message = "Role deleted successfully";
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.Message = "Failed to delete role";
                            response.IsSuccess = false;
                            response.StatusCode = HttpStatusCode.InternalServerError;
                        }
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Message = $"An error occurred: {ex.Message}";
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                return response;
            }
        }



        // *********************** Filter Method Start   *********************************//



        static Expression<Func<AspNetRoles, bool>> CombineLambdas(Expression<Func<AspNetRoles, bool>> expr1, Expression<Func<AspNetRoles, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(AspNetRoles));
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

            return Expression.Lambda<Func<AspNetRoles, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterRole(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _roleManager.Roles.Where(x => x.IsDeleted != true && x.Name!= "SysAdmin" &&x.Name!= "beOnAdmin").AsQueryable();
                // Loop through each filter

                Expression<Func<AspNetRoles, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(AspNetRoles));
                Expression<Func<AspNetRoles, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<AspNetRoles, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<AspNetRoles, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<AspNetRoles, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<AspNetRoles, string>>(SortColumn, parameter);
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
                var roles= await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).ToListAsync();

                ResponseRoleDto Response = new ResponseRoleDto()
                {
                    Roles = roles,
                    TotalRecord = totalRecord
                };

                if (roles == null)
                {
                    response.Message = "No Any Role";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Role Get Sucesfully";
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