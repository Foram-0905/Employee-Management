using beONHR.DAL;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface IPermissionService
    {
        Task<ClientResponse> SetPermission(SetPermissionDTO input);
        Task<ClientResponse> GetPermissionByRole(Guid id);
    }

    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepo _permission;
        public PermissionService(IPermissionRepo permission)
        {
            _permission = permission;
        }

        public async Task<ClientResponse> SetPermission(SetPermissionDTO input)
        {
            try
            {
                return await _permission.SetPermission(input);     
            }
            catch (Exception)
            {
                throw;
            }
        } 
        
        public async Task<ClientResponse> GetPermissionByRole(Guid id)
        {
            try
            {
                return await _permission.GetPermissionByRole(id);     
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
