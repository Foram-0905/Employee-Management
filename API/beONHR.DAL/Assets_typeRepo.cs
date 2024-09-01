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

namespace beONHR.DAL
{
    public interface IAssets_typeRepo
    {

        Task<ClientResponse> GetAssets_type();
    }

    public class Assets_typeRepo : IAssets_typeRepo
    {
        private readonly MainContext _context;

        public Assets_typeRepo(MainContext context)
        {
            _context = context;
        }



        public async Task<ClientResponse> GetAssets_type()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var assets = await _context.Assets_Type.Where(x => x.IsDeleted != true ).OrderBy(x => x.AssetTypes).ToListAsync();

                response.Message = "Assets_type retrieved successfully";
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
    }
}