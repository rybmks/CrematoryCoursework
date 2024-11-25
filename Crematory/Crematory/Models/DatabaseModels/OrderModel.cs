using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models.DatabaseModels
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateOnly OrderDate { get; set; }
        public int ContactPersonId { get; set; }
        public int DeceasedId { get; set; }
        public int CrematoryId { get; set; }
        public DateTime CremationDateTime { get; set; }
        public TimeSpan CremationDuration { get; set; }
        public decimal StandardPrice { get; set; }
    }
}
