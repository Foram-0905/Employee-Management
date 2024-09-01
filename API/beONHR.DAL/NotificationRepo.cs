using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface INotificationRepo
    {
        Task<ClientResponse> GetNotification();
        Task<ClientResponse> GetNotificationByEmployeeId(Guid employeeId);
        Task<ClientResponse> SaveNotification(NotificationDTO input);
        Task<ClientResponse> UpdateNotification(NotificationDTO input);

        Task<ClientResponse> GetNotificationId(Guid notificationID);
    }

    public class NotificationRepo : INotificationRepo
    {
        private readonly MainContext _context;

        public NotificationRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveNotification(NotificationDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {

                var notification = new Notification
                {
                    NotificationID = Guid.NewGuid(),
                    EmployeeId = input.EmployeeId,
                    NotificationText = input.NotificationText,
                    NotificationType = input.NotificationType,
                    IsRead = input.IsRead,
                    CreatedAt = DateTime.Now // Ensure this is always set
                };

                await _context.Notifications.AddAsync(notification);
                var result = await _context.SaveChangesAsync();

                if (result == 0)
                {
                    response.Message = "Notification not saved";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Notification saved successfully";
                    response.HttpResponse = notification.NotificationID;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetNotification()
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var notifications = await _context.Notifications.Include(x => x.Employee).Where(x => x.IsRead != true).ToListAsync();

                if (notifications == null)
                {
                    response.Message = "No Notifications";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                }
                else
                {
                    response.Message = "Notifications Retrieved Successfully";
                    response.HttpResponse = notifications;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetNotificationByEmployeeId(Guid employeeId)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var notifications = await _context.Notifications
                                                  .Where(x => x.EmployeeId == employeeId && x.IsRead != true)
                                                  .OrderByDescending(x => x.CreatedAt) // Added this line to order by CreatedAt in descending order
                                                  .Select(x => new NotificationDTO
                                                  {
                                                      NotificationID = x.NotificationID,
                                                      NotificationText = x.NotificationText,
                                                      NotificationType = x.NotificationType,
                                                      IsRead = x.IsRead,
                                                      EmployeeId = x.EmployeeId,
                                                      CreatedAt = x.CreatedAt
                                                  })
                                                  .ToListAsync();

                if (notifications == null || !notifications.Any())
                {
                    response.Message = "No Notifications Found for the Employee";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.NoContent;
                }
                else
                {
                    response.Message = "Notifications Retrieved Successfully";
                    response.HttpResponse = notifications;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ClientResponse> UpdateNotification(NotificationDTO input)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var existingNotification = await _context.Notifications
                    .FirstOrDefaultAsync(n => n.NotificationID == input.NotificationID);

                if (existingNotification == null)
                {
                    response.Message = "Notification not found";
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    return response;
                }

                // Log the existing notification details
                Console.WriteLine($"Existing Notification: {existingNotification.NotificationID}, {existingNotification.NotificationText}");

                existingNotification.EmployeeId = input.EmployeeId;
                existingNotification.NotificationID = input.NotificationID;
                existingNotification.NotificationText = input.NotificationText;
                existingNotification.NotificationType = input.NotificationType;
                existingNotification.IsRead = input.IsRead=true;
                existingNotification.CreatedAt = DateTime.Now; // Ensure this is always set

                _context.Notifications.Update(existingNotification);

                // Log before saving changes
                Console.WriteLine("Saving changes to the database...");

                var result = await _context.SaveChangesAsync();

                // Log the result of save changes
                Console.WriteLine($"SaveChangesAsync result: {result}");

                if (result == 0)
                {
                    response.Message = "Notification not updated";
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.IsSuccess = false;
                }
                else
                {
                    response.Message = "Notification updated successfully";
                    response.HttpResponse = existingNotification.NotificationID;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
                response.Message = "An error occurred";
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                return response;
            }
        }

        public async Task<ClientResponse> GetNotificationId(Guid notificationID)
        {
            ClientResponse response = new ClientResponse();
            try
            {
                var notifications = await _context.Notifications
                                                  .Where(x => x.NotificationID == notificationID && x.IsRead != true).Select(x => new NotificationDTO
                                                  {
                                                      NotificationID= x.NotificationID,
                                                      NotificationText= x.NotificationText,
                                                      NotificationType= x.NotificationType,
                                                      IsRead= x.IsRead,
                                                      EmployeeId = x.EmployeeId,
                                                      CreatedAt = x.CreatedAt
                                                  })
                                                  .ToListAsync();

                if (notifications == null || !notifications.Any())
                {
                    response.Message = "No Notifications Found for the NotificationId";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Notifications Retrieved Successfully";
                    response.HttpResponse = notifications;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
