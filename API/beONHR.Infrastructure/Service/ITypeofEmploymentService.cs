using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface ITypeofEmploymentService
    {
        Task<ClientResponse> GetTypeofEmployment();
    }
    public class TypeofEmploymentService : ITypeofEmploymentService
    {
        private readonly ITypeofEmploymentRepo _typeofemploymentRepo;

        public TypeofEmploymentService(ITypeofEmploymentRepo typeofemploymentRepo)
        {
            _typeofemploymentRepo = typeofemploymentRepo;
        }



        public async Task<ClientResponse> GetTypeofEmployment()
        {
            try
            {
                return await _typeofemploymentRepo.GetTypeofEmployment();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
