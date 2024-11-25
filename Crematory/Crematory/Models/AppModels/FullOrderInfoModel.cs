using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models.AppModels
{
    public class FullOrderInfoModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CremationDateTime { get; set; }
        public TimeSpan CremationDuration { get; set; }
        public int DeceasedId { get; set; }
        public string? DeceasedName { get; set; }
        public int ContactPersonId { get; set; }
        public string? ContactPersonName { get; set; }
        public int CrematoryId { get; set; }
        public string? CrematoryName { get; set; }
        public decimal TotalPrice { get; set; }
        public string? CompletionReason { get; set; }
    }
}
