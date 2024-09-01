using beONHR.DAL;
using beONHR.Entities.DTO.ForgotPassword;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using beONHR.Entities;

namespace beONHR.Infrastructure.Service
{
    public interface IDesignationService
    {
        Task<ClientResponse> SaveDesignation(DesignationDto input);
        Task<ClientResponse> GetDesignation();
        Task<ClientResponse> GetFilterDesignation(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteDesignation(Guid id);
        Task<ClientResponse> GetDesignationById(Guid id);
    }
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepo _designation;
        public DesignationService(IDesignationRepo designation)
        {
            _designation = designation;
        }

        public async Task<ClientResponse> SaveDesignation(DesignationDto input)
        {
            try
            {
                return await _designation.SaveDesignation(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<ClientResponse> GetDesignation()
        {
            try
            {
                return await _designation.GetDesignation();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterDesignation(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _designation.GetFilterDesignation(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteDesignation(Guid id)
        {
            try
            {
                return await _designation.DeleteDesignation(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        public async Task<ClientResponse> GetDesignationById(Guid id)
        {
            try
            {
                return await _designation.GetDesignationById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
