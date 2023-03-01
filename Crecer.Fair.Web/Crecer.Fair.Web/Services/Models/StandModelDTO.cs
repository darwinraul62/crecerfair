using System;
using System.Collections.Generic;

namespace Crecer.Fair.Web.Services.Models
{
    public class StandModelViewModelDTO
    {
        public string ModelCode { get; set; }
        public string Name { get; set; }
        public List<StandModelResourceViewModelDTO> Resources { get; set; }
    }
    public class StandModelResourceViewModelDTO
    {        
        public string ResourceTypeId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
