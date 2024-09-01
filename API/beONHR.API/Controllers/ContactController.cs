using beONHR.Entities;
using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ContactController : ControllerBase
    {
        private readonly IContactService _contact;
        public ContactController(IContactService contact)
        {
            _contact = contact;
        }

        [HttpPost]
        [Route("SaveContact")]
    

        public async Task<IActionResult> SaveContact(ContactDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _contact.SaveContact(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [Route("UpdateContact")]

        public async Task<IActionResult> UpdateContact(ContactDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _contact.UpdateContact(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetContactByEmployeeId/{eid}")]
        public async Task<ClientResponse> GetContactByEmployeeId(Guid eid)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _contact.GetContactByEmployeeId(eid);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetContact")]
        public async Task<ClientResponse> GetContact()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _contact.GetContact();

                return objresp; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetContactById/{Id}")]
        public async Task<ClientResponse> GetContactById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _contact.GetContactById(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected IActionResult returnAction(ClientResponse objresp)
        {
            if (objresp.StatusCode == HttpStatusCode.OK || objresp.StatusCode == HttpStatusCode.NoContent)
            {
                return Ok(objresp);
            }
            else
            {
                return BadRequest(objresp);
            }
        }
    }
}
