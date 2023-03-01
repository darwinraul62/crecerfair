using System;

namespace Crecer.Fair.Data.Models
{
    public class FairStand
    {
        public Guid StandId { get; set; }
        public Guid FairId { get; set; }
        public Guid? ProviderId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ModelCode { get; set; }
        public bool IsEditable { get; set; }
        public int PositionRef { get; set; }
        public FairModel Fair { get; set; }
        public Provider Provider { get; set; }
    }
}
