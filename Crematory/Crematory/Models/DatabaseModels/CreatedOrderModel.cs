using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models
{
    public class CreatedOrderModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateOnly CompletionDate { get; set; }
        public string CompletionReason { get; set; } = string.Empty;
    }
}
