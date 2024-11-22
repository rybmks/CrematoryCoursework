using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models.AppModels
{
    public class DisplayOrderModel
    {
        public int Id { get; set; }
        public DateTime CremationDateTime { get; set; }
        public string CrematoryName { get; set; }
    }
}
