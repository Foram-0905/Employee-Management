using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;

namespace beONHR.DAL
{
    public interface ITransactionRepo
    {
        Task<ClientResponse> GetTransaction();

    }
    public class TransactionTypeRepo : ITransactionRepo
    {
        private readonly MainContext _context;
        public TransactionTypeRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> GetTransaction()
        {
            ClientResponse response = new ClientResponse();

            try
            {
                var transactions = await _context.TransactionTypes
                       .Where(x => x.IsDeleted != true)
                    .ToListAsync();

                if (transactions == null || !transactions.Any())
                {
                    response.Message = "No transactions found.";
                    response.HttpResponse = null;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    response.Message = "Transactions retrieved successfully.";
                    response.HttpResponse = transactions;
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                }

                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
