using Crematory.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crematory.Models.DatabaseModels
{
    public class DeceasedModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public DateOnly DeathDate { get; set; }
        public Gender Gender { get; set; }
    }
}
