using beONHR.DAL;
using beONHR.Entities;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    public interface ITransactionService
    {
        Task<ClientResponse> GetTransaction();

    }
    public class TransactionTypeService:ITransactionService
    {
        private readonly ITransactionRepo _transaction;
        public TransactionTypeService(ITransactionRepo transaction)
        {
            _transaction = transaction;
        }

        public async Task<ClientResponse> GetTransaction()
        {
            try
            {
                return await _transaction.GetTransaction();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
