using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models
{
    public class CrematoryScheduleModel
    {
        public int Id { get; set; }
        public int CrematoryId { get; set; }
        public string? DayOfWeek { get; set; }
        private TimeSpan _openTime;
        private TimeSpan _closeTime;
        public TimeSpan OpenTime
        {
            get => _openTime;
            set => _openTime = new TimeSpan(value.Hours, value.Minutes, 0); 
        }
        public TimeSpan CloseTime
        {
            get => _closeTime;
            set => _closeTime = new TimeSpan(value.Hours, value.Minutes, 0);
        }
    }

}
