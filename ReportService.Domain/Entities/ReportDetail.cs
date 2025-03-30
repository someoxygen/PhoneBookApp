using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Domain.Entities
{
    public class ReportDetail
    {
        public string Location { get; set; } = null!;
        public int PersonCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
