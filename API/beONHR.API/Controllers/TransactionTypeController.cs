using beONHR.Entities.DTO;
using beONHR.Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static beONHR.Entities.Permissions;

namespace beONHR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionTypeController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionTypeController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("GetTransactionTypes")]
        public async Task<IActionResult> GetTransaction()
        {
            try
            {
                var response = await _transactionService.GetTransaction();
                return Ok(response); 
            }
            catch (Exception ex)
            {
                // Log the exception here
                return StatusCode(500, "Internal server error"); 
            }
        }
    }
}
