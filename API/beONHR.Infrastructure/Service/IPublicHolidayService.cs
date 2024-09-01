using beONHR.DAL;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.ForgotPassword;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IPublicHolidayService
    {
        Task<ClientResponse> SavePublicHoliday(PublicHolidayDTO input);
        Task<ClientResponse> GetPublicHoliday();
        Task<ClientResponse> DeletePublicHoliday(Guid id);
        Task<ClientResponse> GetPublicHolidayById(Guid id);
        Task<ClientResponse> GetFilterPublicHoliday(FilterRequsetDTO filterRequset);

    }

    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly IPublicHolidayRepo _publicHoliday;

        public PublicHolidayService(IPublicHolidayRepo publicHoliday)
        {
            _publicHoliday = publicHoliday;
        }

        public async Task<ClientResponse> SavePublicHoliday(PublicHolidayDTO input)
        {
            try
            {
                return await _publicHoliday.SavePublicHoliday(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterPublicHoliday(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _publicHoliday.GetFilterPublicHoliday(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetPublicHoliday()
        {
            try
            {
                return await _publicHoliday.GetPublicHoliday();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> DeletePublicHoliday(Guid id)
        {
            try
            {
                return await _publicHoliday.DeletePublicHoliday(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetPublicHolidayById(Guid id)
        {
            try
            {
                return await _publicHoliday.GetPublicHolidayById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}