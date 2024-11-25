using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models.DatabaseModels
{
    public class CompletedOrderModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string? CompetionReason { get; set; }
    }
}
