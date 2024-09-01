using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.Infrastructure.Service
{
    public interface IDocumentService
    {
        Task<ClientResponse> SaveDocument(DocumentDTO input);
        Task<ClientResponse> GetDocument();
        Task<ClientResponse> DeleteDocument(Guid id);
        Task<ClientResponse> GetDocumentById(Guid id);
    }
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepo _document;
        public DocumentService(IDocumentRepo document)
        {
            _document = document;
        }

        public async Task<ClientResponse> SaveDocument(DocumentDTO input)
        {
            try
            {
                return await _document.SaveDocument(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ClientResponse> GetDocument()
        {
            try
            {
                return await _document.GetDocument();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<ClientResponse> DeleteDocument(Guid id)
        {
            try
            {
                return await _document.DeleteDocument(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ClientResponse> GetDocumentById(Guid id)
        {
            try
            {
                return await _document.GetDocumentById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
