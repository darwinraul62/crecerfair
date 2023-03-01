using System;
using System.Collections.Generic;

namespace Crecer.Fair.Data.Models
{
    public class StandModel
    {
        public string ModelCode { get; set; }
        public string Name { get; set; }
        public List<StandModelResource> Resources { get; set; }
    }
}
