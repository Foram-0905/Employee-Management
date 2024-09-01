using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface IDocumentListService
    {
        Task<ClientResponse> SaveDocumentList(DocumentListDTO input);
        Task<ClientResponse> GetDocumentList();
        Task<ClientResponse> GetFilterDocumentList(FilterRequsetDTO filterRequset);
        Task<ClientResponse> DeleteDocumentList(Guid id);
        Task<ClientResponse> GetDocumentListById(Guid id);
        Task<ClientResponse> GetDocumentListByEmployeeIdOrEntityId(Guid? employeeId = null, Guid? entityId = null, string? fileName = null);
    }
    public class DocumentListService : IDocumentListService
    {
        private readonly IDocumenListRepo _documentlist;
        public DocumentListService(IDocumenListRepo document)
        {
            _documentlist = document;
        }

        public async Task<ClientResponse> SaveDocumentList(DocumentListDTO input)
        {
            try
            {
                return await _documentlist.SaveDocumentList(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
 
        public async Task<ClientResponse> GetDocumentList()
        {
            try
            {
                return await _documentlist.GetDocumentList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> GetFilterDocumentList(FilterRequsetDTO filterRequset)
        {
            try
            {
                return await _documentlist.GetFilterDocumentList(filterRequset);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteDocumentList(Guid id)
        {
            try
            {
                return await _documentlist.DeleteDocumentList(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<ClientResponse> GetDocumentListById(Guid Id)
        {
            try
            {
                return await _documentlist.GetDocumentListById(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetDocumentListByEmployeeIdOrEntityId(Guid? employeeId = null, Guid? entityId = null, string? fileName = null)
        {
            try
            {
                return await _documentlist.GetDocumentListByEmployeeIdOrEntityId(employeeId, entityId,fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
