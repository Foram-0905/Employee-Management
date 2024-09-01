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
    public interface ItaxclassRepo
    {

        Task<ClientResponse> Gettaxclass();
    }

    public class taxclassRepo : ItaxclassRepo
    {
        private readonly MainContext _context;

        public taxclassRepo(MainContext context)
        {
            _context = context;
        }



        public async Task<ClientResponse> Gettaxclass()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var tax = await _context.TaxClass.Where(x => x.IsDeleted != true).ToListAsync();

                response.Message = "Tax Class retrieved successfully";
                response.HttpResponse = tax;
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