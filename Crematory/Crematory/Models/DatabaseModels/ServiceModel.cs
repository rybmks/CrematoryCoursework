using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Crematory.Models
{
    public class ServiceModel 
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public bool IsSelected { get; set; }
    }
}
