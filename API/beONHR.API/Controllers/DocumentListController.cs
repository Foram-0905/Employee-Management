using beONHR.Entities;
using beONHR.Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DocumentListController : Controller
    {
        private readonly IDocumentListService _documentlist;
        public DocumentListController(IDocumentListService document)
        {
            _documentlist = document;
        }

        [HttpPost]
        [Route("SaveDocumentList")]
        [Authorize("employeeprofile.documentlist.Add")]


        public async Task<IActionResult> SaveDocumentList(DocumentListDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _documentlist.SaveDocumentList(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetDocumentList")]
        public async Task<ClientResponse> GetDocumentList()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _documentlist.GetDocumentList();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("GetFilterDocumentList")]
        public async Task<ClientResponse> GetFilterDocumentList(FilterRequsetDTO filterRequset)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _documentlist.GetFilterDocumentList(filterRequset);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteDocumentList/{Id}")]
        [Authorize("employeeprofile.documentlist.Delete")]
        public async Task<ClientResponse> DeleteDocumentList(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _documentlist.DeleteDocumentList(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("GetDocumentListById/{Id}")]
        public async Task<ClientResponse> GetDocumentListById(Guid Id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _documentlist.GetDocumentListById(Id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetDocumentListByEmployeeId/{employeeId}")]
        public async Task<ClientResponse> GetDocumentListByEmployeeId(Guid employeeId)
        {
            try
            {
                return await _documentlist.GetDocumentListByEmployeeIdOrEntityId(employeeId: employeeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetDocumentListByEntityId/{entityId}")]
        public async Task<ClientResponse> GetDocumentListByEntityId(Guid entityId, [FromQuery] string? fileName = null)
        {
            try
            {
                return await _documentlist.GetDocumentListByEmployeeIdOrEntityId(entityId: entityId, fileName:fileName);
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
