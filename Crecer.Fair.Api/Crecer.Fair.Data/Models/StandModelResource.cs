using System;

namespace Crecer.Fair.Data.Models
{
    public class StandModelResource
    {
        public string ModelCode { get; set; }
        public string ResourceTypeId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public StandModel StandModel { get; set; }
    }
}
