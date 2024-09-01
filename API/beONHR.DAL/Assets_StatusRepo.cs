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
    public interface IAssets_StatusRepo
    {

        Task<ClientResponse> GetAssets_Status();
    }

    public class Assets_StatusRepo : IAssets_StatusRepo
    {
        private readonly MainContext _context;

        public Assets_StatusRepo(MainContext context)
        {
            _context = context;
        }



        public async Task<ClientResponse> GetAssets_Status()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var assets = await _context.Assets_Status.Where(x => x.IsDeleted != true).ToListAsync();

                response.Message = "Assets_Status retrieved successfully";
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