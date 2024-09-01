using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface ITypeofEmploymentRepo
    {
        Task<ClientResponse> GetTypeofEmployment();
    }

    public class TypeofEmploymentRepo : ITypeofEmploymentRepo
    {
        private readonly MainContext _context;

        public TypeofEmploymentRepo(MainContext context)
        {
            _context = context;
        }



        public async Task<ClientResponse> GetTypeofEmployment()
        {
            ClientResponse response = new();
            try
            {
                // Get all assets
                var Employment = await _context.TypeofEmployments.Where(x => x.IsDeleted != true).OrderBy(x => x.typeofemployment).ToListAsync();

                response.Message = "TypeofEmployment retrieved successfully";
                response.HttpResponse = Employment;
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
