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
    public interface IRoleService
    {
        Task<ClientResponse> SaveRole(RoleDTO input);
        Task<ClientResponse> GetRole();
        Task<ClientResponse> GetFilterRole(FilterRequsetDTO filterRequset);
        Task<ClientResponse> GetRoleById(Guid id);
        Task<ClientResponse> DeleteRole(string id);
    }

    public class RoleService : IRoleService
    {
        private readonly IRoleRepo _role;
        public RoleService(IRoleRepo role)
        {
            _role = role;
        }

        public async Task<ClientResponse> SaveRole(RoleDTO input)
        {
            try
            {
                return await _role.SaveRole(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetRole()
        {
            try
            {
                return await _role.GetRole();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
 
        public async Task<ClientResponse> GetRoleById(Guid id)
        {
            try
            {
                return await _role.GetRoleById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterRole(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _role.GetFilterRole(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteRole(string id)
        {
            try
            {
                return await _role.DeleteRole(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}