// AssetsRepo.cs
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
using static beONHR.Entities.Permissions;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace beONHR.DAL
{
    public interface IAssetsRepo
    {
        Task<ClientResponse> SaveAsset(AssetsDTO input);
        Task<ClientResponse> GetAssets();
        Task<ClientResponse> GetFilterAssets(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteAsset(Guid id);
        Task<ClientResponse> GetAssetById(Guid id);
        Task<ClientResponse> GetAssetByEmployeeId(Guid id);
    }

    public class AssetsRepo : IAssetsRepo
    {
        private readonly MainContext _context;

        public AssetsRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveAsset(AssetsDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input == null)
                {
                    response.Message = "Asset data is null";
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    return response;
                }

                // Check if the asset with the given ID exists
                var existingAsset = await _context.ManageAssets.FindAsync(input.Id);

                if (existingAsset == null)
                {
                    // Create a new asset
                    ManageAssets newAsset = new ManageAssets
                    {
                        SerialNumber = input.SerialNumber,
                        Status = input.Status,
                        Manufacturer = input.Manufacturer,
                        AssetType = input.AssetType,
                        Model = input.Model,
                        MoreDetails = input.MoreDetails,
                        PurchaseDate = input.PurchaseDate,
                        WarrantyDueDate = input.WarrantyDueDate,
                        Warranty = input.Warranty,
                       CurrentOwner = input.CurrentOwner,
                        PreviousOwner = input.PreviousOwner,
                        Note = input.Note
                    };

                    _context.ManageAssets.Add(newAsset);
                    await _context.SaveChangesAsync();

                    response.Message = "Asset saved successfully";
                    response.HttpResponse = newAsset.Id;
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = true;
                }
                else
                {
                    // Update existing asset
                    existingAsset.SerialNumber = input.SerialNumber;
                    existingAsset.Status = input.Status;
                    existingAsset.Manufacturer = input.Manufacturer;
                    existingAsset.AssetType = input.AssetType;
                    existingAsset.Model = input.Model;
                    existingAsset.MoreDetails = input.MoreDetails;
                    existingAsset.PurchaseDate = input.PurchaseDate;
                    existingAsset.WarrantyDueDate = input.WarrantyDueDate;
                    existingAsset.Warranty = input.Warranty;
                    existingAsset.CurrentOwner = input.CurrentOwner;
                    existingAsset.PreviousOwner = input.PreviousOwner;
                    existingAsset.Note = input.Note;

                    _context.ManageAssets.Update(existingAsset);
                    await _context.SaveChangesAsync();

                    response.Message = "Asset updated successfully";
                    response.HttpResponse = existingAsset.Id;
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

        public async Task<ClientResponse> GetAssets()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                // Get all assets
                var assets = await _context.ManageAssets.Where(x => x.IsDeleted != true).Select(e => new AssetsDTO
                {
                    Id = e.Id,
                    SerialNumber= e.SerialNumber,
                    Status= e.Status,
                    StatusName=e.Statusname.Status,
                    Manufacturer = e.Manufacturer,
                    AssetType = e.AssetType,
                    AssetTypeName=e.AssetTypename.AssetTypes,
                    Model = e.Model,
                    MoreDetails = e.MoreDetails,
                    PurchaseDate = e.PurchaseDate,
                    WarrantyDueDate = e.WarrantyDueDate,
                    //Warranty = e.Warranty,
                    CurrentOwner = e.CurrentOwner,
                    CurrentOwnerFullName=e.CurrentOwnerEmployee.FirstName+' '+ e.CurrentOwnerEmployee.MiddleName + ' ' + e.CurrentOwnerEmployee.LastName,
                    PreviousOwner = e.PreviousOwner,
                    PreviousOwnerFullName=e.PreviousOwnerEmployee.FirstName+ ' ' + e.PreviousOwnerEmployee.MiddleName+ ' ' + e.PreviousOwnerEmployee.LastName,
                    Note = e.Note,
                }).ToListAsync();

                if (assets == null)
                {
                    response.Message = "No Assests Found";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                response.Message = "Assets retrieved successfully";
                response.HttpResponse = assets;
                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;     

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeleteAsset(Guid id)
        {
            ClientResponse response = new();
            try
            {
                // Find the asset by its ID and ensure it's not already deleted
                var asset = await _context.ManageAssets
                                           .Where(x => x.Id == id && !x.IsDeleted)
                                           .FirstOrDefaultAsync();
                if (asset == null)
                {
                    response.Message = "Asset not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }
                else if (asset.CurrentOwner.HasValue)
                {
                    // Check if CurrentOwner has a value
                    response.Message = "Asset cannot be deleted  because it has a current owner";
                    response.StatusCode = HttpStatusCode.OK;
                    response.IsSuccess = false;
                }
                else
                {
                    // Soft delete the asset by updating the IsDeleted property
                    asset.IsDeleted = true;

                    _context.ManageAssets.Update(asset);
                    var res = await _context.SaveChangesAsync();

                    if (res == 0)
                    {
                        response.Message = "Asset deletion failed";
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = "Asset deleted successfully";
                        response.IsSuccess = true;
                        response.StatusCode = HttpStatusCode.OK;
                      
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




        public async Task<ClientResponse> GetAssetById(Guid id)
        {
            ClientResponse response = new();
            try
            {
                // Get the asset by ID
                var asset = await _context.ManageAssets.FindAsync(id);

                if (asset == null)
                {
                    response.Message = "Asset not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Asset retrieved successfully";
                    response.HttpResponse = asset;
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

        public async Task<ClientResponse> GetAssetByEmployeeId(Guid id)
        {

            ClientResponse response = new();
            try
            {

                var asset = await _context.ManageAssets.Where(x => x.CurrentOwner == id && x.IsDeleted != true).FirstOrDefaultAsync();

                if (asset != null)
                {

                    response.Message = "asset Get Sucesfully";
                    response.HttpResponse = asset;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;


                }
                else
                {
                    response.Message = "asset not Exists";
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


        static Expression<Func<ManageAssets, bool>> CombineLambdas(Expression<Func<ManageAssets, bool>> expr1, Expression<Func<ManageAssets, bool>> expr2, FilterRequsetDTO filterRequset)
        {
            var parameter = Expression.Parameter(typeof(ManageAssets));
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

            return Expression.Lambda<Func<ManageAssets, bool>>(body, parameter);
        }
        public async Task<ClientResponse> GetFilterAssets(FilterRequsetDTO filterRequset)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var query = _context.ManageAssets.Where(x => x.IsDeleted != true).Include(x => x.CurrentOwnerEmployee).Include(a => a.Statusname).Include(a => a.AssetTypename).AsQueryable();
                // Loop through each filter

                Expression<Func<ManageAssets, bool>> combinedCondition = null;

                var parameter = Expression.Parameter(typeof(ManageAssets));
                Expression<Func<ManageAssets, bool>> lambda = null;

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
                            lambda = Expression.Lambda<Func<ManageAssets, bool>>(condition, parameter);

                            //query = query.Where(lambda);

                        }
                        else
                        {
                            var property = Expression.Property(parameter, filter.Key);
                            var value = Expression.Constant(filter.Value.filter);
                            var condition = Expression.Call(property, filter.Value.type.First().ToString().ToUpper() + string.Join("", filter.Value.type.Skip(1)), Type.EmptyTypes, value);
                            lambda = Expression.Lambda<Func<ManageAssets, bool>>(condition, parameter);
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

                    var sortCodition = Expression.Lambda<Func<ManageAssets, string>>(sortTableCol, parameter);

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
                        var sortCodition = Expression.Lambda<Func<ManageAssets, string>>(SortColumn, parameter);
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
                var manageAssets = await query
                            .Skip(skip)
                            .Take((int)(filterRequset.PageSize)).Select(e => new AssetsDTO
                            {
                                Id = e.Id,
                                SerialNumber = e.SerialNumber,
                                Status = e.Status,
                                StatusName = e.Statusname.Status,
                                Manufacturer = e.Manufacturer,
                                AssetType = e.AssetType,
                                AssetTypeName = e.AssetTypename.AssetTypes,
                                Model = e.Model,
                                MoreDetails = e.MoreDetails,
                                PurchaseDate = e.PurchaseDate,
                                WarrantyDueDate = e.WarrantyDueDate,
                                //Warranty = e.Warranty,
                                CurrentOwner = e.CurrentOwner,
                                CurrentOwnerFullName = e.CurrentOwnerEmployee.FirstName + e.CurrentOwnerEmployee.MiddleName + e.CurrentOwnerEmployee.LastName,
                                PreviousOwner = e.PreviousOwner,
                                PreviousOwnerFullName = e.PreviousOwnerEmployee.FirstName + e.PreviousOwnerEmployee.MiddleName + e.PreviousOwnerEmployee.LastName,
                                Note = e.Note,
                            }).ToListAsync();

                ResponseAssetsDto Response = new ResponseAssetsDto()
                {
                    Assets = manageAssets,
                    TotalRecord = totalRecord
                };

                if (manageAssets == null)
                {
                    response.Message = "No Any Assets";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Assets Get Successfully";
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
