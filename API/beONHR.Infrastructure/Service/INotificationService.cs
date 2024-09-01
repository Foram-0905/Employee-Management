using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{

    public interface INotificationService
    {
        Task<ClientResponse> SaveNotification(NotificationDTO input);
        Task<ClientResponse> GetNotification();
        Task<ClientResponse> GetNotificationByEmployeeId(Guid employeeId);
        Task<ClientResponse> UpdateNotification(NotificationDTO input);
        Task<ClientResponse> GetNotificationId(Guid notificationID);
   

    }


    public class NotificationService : INotificationService
        {
            private readonly INotificationRepo _notificationRepo;

            public NotificationService(INotificationRepo notificationRepo)
            {
                _notificationRepo = notificationRepo;
            }

            public async Task<ClientResponse> SaveNotification(NotificationDTO input)
            {
                try
                {
                    return await _notificationRepo.SaveNotification(input);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<ClientResponse> GetNotification()
            {
                try
                {
                    return await _notificationRepo.GetNotification();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public async Task<ClientResponse> GetNotificationByEmployeeId(Guid employeeId)
            {
                try
                {
                    return await _notificationRepo.GetNotificationByEmployeeId(employeeId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        public async Task<ClientResponse> GetNotificationId(Guid notificationID)
        {
            try
            {
                return await _notificationRepo.GetNotificationId(notificationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> UpdateNotification(NotificationDTO input)
        {
            try
            {
                return await _notificationRepo.UpdateNotification(input);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
       
    }
}


    

