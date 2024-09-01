using beONHR.DAL;
using beONHR.Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Infrastructure.Service
{
    //public interface IProbationService
    //{
    //    Task<ClientResponse> SaveProbation(ProbationDTO input);
    //    Task<ClientResponse> DeleteProbation(Guid id);
    //    Task<ClientResponse> GetProbation();
    //}
    //public class ProbationService : IProbationService
    //{
    //    private readonly IProbationRepo _providerrepo;
    //    public ProbationService(IProbationRepo Providerrepo)
    //    {
    //        _providerrepo = Providerrepo;
    //    }

    //    public async Task<ClientResponse> SaveProbation(ProbationDTO input)
    //    {
    //        try
    //        {
    //            return await _providerrepo.SaveProbation(input);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public async Task<ClientResponse> GetProbation()
    //    {
    //        try
    //        {
    //            return await _providerrepo.GetProbation();
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }

    //    public async Task<ClientResponse> DeleteProbation(Guid id)
    //    {
    //        try
    //        {
    //            return await _providerrepo.DeleteProbation(id);
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //    }
    //}
}
