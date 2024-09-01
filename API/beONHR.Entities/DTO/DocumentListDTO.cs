using beONHR.Entities.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beONHR.Entities.DTO
{
    public class DocumentListDTO
    {
        public Guid Id { get; set; }
        public string TabName { get; set; }
        public string Modulename { get; set; }
        public string FileName { get; set; }
        public string Documents { get; set; }
        public Guid EmployeeId { get; set; } //forignkey
        public ActionEnum Action { get; set; }
    }

    public class ResponseDocumentListDto
    {
        public List<DocumentListDTO> documentlists { get; set; }
        public int TotalRecord { get; set; }
    }

}
