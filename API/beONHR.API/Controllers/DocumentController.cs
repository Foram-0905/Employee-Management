using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : Controller
    {
        private readonly IDocumentService _document;
        public DocumentController(IDocumentService document)
        {
            _document = document;
        }


        [HttpPost]
        [Route("SaveDocument")]
        [Authorize("employeeprofile.document.Add")]

        public async Task<IActionResult> SaveDocument(DocumentDTO input)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _document.SaveDocument(input);

                return Ok(objresp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetDocument")]
        public async Task<ClientResponse> GetDocument()
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _document.GetDocument();

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete]
        [Route("DeleteDocument/{Id}")]
        [Authorize("employeeprofile.document.Delete")]
        public async Task<ClientResponse> DeleteDocument(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {
                objresp = await _document.DeleteDocument(id);

                return objresp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetDocumentById/{Id}")]
        public async Task<ClientResponse> GetDocumentById(Guid id)
        {
            ClientResponse objresp = new ClientResponse();
            try
            {

                objresp = await _document.GetDocumentById(id);

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
