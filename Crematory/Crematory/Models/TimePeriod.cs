using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models
{
    public class TimePeriod 
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public TimeOnly StartAsTimeOnly() => TimeOnly.FromTimeSpan(StartTime);
        public TimeOnly EndAsTimeOnly() => TimeOnly.FromTimeSpan(EndTime);

    }
}
