using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IIdentityService
    {
        Task<ClientResponse> SaveIdentity(IdentityCardDTO input);
        Task<ClientResponse> GetIdentity();
        Task<ClientResponse> DeleteIdentity(Guid id);
        Task<ClientResponse> GetIdentityById(Guid id);
        Task<ClientResponse> GetIdentityByEmployeeId(Guid id);
    }
    public class IdentityCardService : IIdentityService
    {
        private readonly IIdentityRepo _identity;
        public IdentityCardService(IIdentityRepo identity)
        {
            _identity = identity;
        }

        public async Task<ClientResponse> SaveIdentity(IdentityCardDTO input)
        {
            try
            {
                return await _identity.SaveIdentity(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetIdentity()
        {
            try
            {
                return await _identity.GetIdentity();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteIdentity(Guid id)
        {
            try
            {
                return await _identity.DeleteIdentity(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetIdentityById(Guid id)
        {
            try
            {
                return await _identity.GetIdentityById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetIdentityByEmployeeId(Guid id)
        {
            try
            {
                return await _identity.GetIdentityByEmployeeId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
