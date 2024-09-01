using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class FilterRequsetDTO
    {
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public SortModel? sortModel { get; set; }
        public int? filterConditionAndOr { get; set; }
        public Dictionary<string, FilterModelObject>? filterModel { get; set; }

    }
    public class FilterModelObject
    {
        [JsonProperty("operator")]
        public string? logicOperator { get; set; }
        public string? filterTo { get; set; }
        public string? type { get; set; }
        public string? filter { get; set; }

        [JsonProperty("dateFrom")]
        public string? dateFrom { get; set; }
        public string? dateTo { get; set; }
        public string? filterType { get; set; }


    }


    public class FilterModel
    {

        public List<string> filterType { get; set; }
        public List<string> type { get; set; }
        public List<string> filter { get; set; }

    }
    public class SortModel
    {
        [JsonProperty("colId")]
        public string colId { get; set; }
        public string sortOrder { get; set; }
    }
}
