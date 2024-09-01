using beONHR.Entities;
using beONHR.Entities.Context;
using beONHR.Entities.DTO;
using beONHR.Entities.DTO.Email;
using beONHR.Entities.DTO.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.DAL
{
    public interface IConsultantRateRepo
    {
        Task<ClientResponse> SaveConsultantRate(ConsultantRateDTO input);

    }
    public class ConsultantRateRepo : IConsultantRateRepo
    {
        private readonly MainContext _context;
        public ConsultantRateRepo(MainContext context)
        {
            _context = context;
        }

        public async Task<ClientResponse> SaveConsultantRate(ConsultantRateDTO input)
        {
            ClientResponse response = new();
            try
            {
                if (input.Action == ActionEnum.Insert)
                {
                    var consultantRate = await _context.ConsultantRates
                        .Where(x => x.Currency == input.Currency && x.EmployeeId == input.EmployeeId).FirstOrDefaultAsync();

                    if (consultantRate == null)
                    {
                        Consultant_Rate model = new Consultant_Rate
                        {
                            Currency = input.Currency,
                            PricePerDayNet = input.PricePerDayNet,
                            PricePerHourNet = input.PricePerHourNet,
                            EmployeeId = input.EmployeeId
                        };

                        await _context.ConsultantRates.AddAsync(model);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "ConsultantRate not inserted";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "ConsultantRate inserted successfully";
                            response.HttpResponse = model.id;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "ConsultantRate already exists";
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.IsSuccess = false;
                    }
                    return response;
                }
                else
                {
                    var consultantRate = await _context.ConsultantRates
                        .Where(x => x.id == input.id)
                        .FirstOrDefaultAsync();

                    if (consultantRate != null)
                    {
                        consultantRate.Currency = input.Currency;
                        consultantRate.PricePerDayNet = input.PricePerDayNet;
                        consultantRate.PricePerHourNet = input.PricePerHourNet;
                        consultantRate.EmployeeId = input.EmployeeId;

                        _context.ConsultantRates.Update(consultantRate);
                        var res = await _context.SaveChangesAsync();

                        if (res == 0)
                        {
                            response.Message = "ConsultantRate not updated";
                            response.StatusCode = HttpStatusCode.NoContent;
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = "ConsultantRate updated successfully";
                            response.HttpResponse = null;
                            response.IsSuccess = true;
                            response.StatusCode = HttpStatusCode.OK;
                        }
                    }
                    else
                    {
                        response.Message = "ConsultantRate does not exist";
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.IsSuccess = false;
                    }
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
