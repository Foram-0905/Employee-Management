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
    public interface IContactService
    {
        Task<ClientResponse> SaveContact(ContactDTO input);
        Task<ClientResponse> GetContactByEmployeeId(Guid eid);
        Task<ClientResponse> UpdateContact(ContactDTO input);
        Task<ClientResponse> GetContact();
        Task<ClientResponse> GetContactById(Guid id);

    }
    public class ContactService : IContactService
    {
        private readonly IContactRepo _contact;

        public ContactService(IContactRepo contact)
        {
            _contact = contact;
        }

        public async Task<ClientResponse> SaveContact(ContactDTO input)
        {
            try
            {
                return await _contact.SaveContact(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetContactByEmployeeId(Guid eid)
        {
            try
            {
                return await _contact.GetContactByEmployeeId(eid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> UpdateContact(ContactDTO input)
        {
            try
            {
                return await _contact.UpdateContact(input);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetContact()
        {
            try
            {
                return await _contact.GetContact();
            }
            catch (Exception ex)
            {
                throw ex;
            }
       
        }
        public async Task<ClientResponse> GetContactById(Guid id)
        {
            try
            {
                return await _contact.GetContactById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
