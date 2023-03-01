using System;
using System.Collections.Generic;

namespace Crecer.Fair.Web.Models
{
    public class StandViewModel
    {
        public StandViewModel()
        {
        }

        public Guid StandId { get; set; }
        public string ProviderName { get; set; }
        public Guid ProviderId { get; set; }
        public string ModelCode { get; set; }

        public string LogoUrl { get; set; }

        public List<StandResourceCoordViewModel> Coords { get; set; } = new List<StandResourceCoordViewModel>();

        public Contact ContactData { get; set; } = new Contact();
        public List<ItemResourceLink> Links { get; set; } = new List<ItemResourceLink>();
        public List<ItemResourcePhoto> Photos { get; set; } = new List<ItemResourcePhoto>();
        public List<ItemResourceContent> Contents { get; set; } = new List<ItemResourceContent>();
        public List<ItemResourceFile> Files { get; set; } = new List<ItemResourceFile>();
        public List<ItemResourceFile> Videos { get; set; } = new List<ItemResourceFile>();
        public List<ItemResourceFile> Banner { get; set; } = new List<ItemResourceFile>();
        public List<ItemResourceCalendar> CalendarItems { get; set; } = new List<ItemResourceCalendar>();
    }

    public class ItemResourceCalendar
    {
        public int WeekDay { get; set; }
        public string WeekDayDescription { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class CoordStandModel
    {
        public StandViewModel StandModel { get; set; }
    }

    public class Contact
    {
        public string Address { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string FacebookAddress { get; set; }
        public string InstagramAddress { get; set; }
        public string TwitterAddress { get; set; }
        public string YoutubeAddress { get; set; }
    }

    public class ItemResourceLink
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Priority { get; set; }
    }

    public class ItemResourcePhoto
    {
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Url { get; set; }
        public int Priority { get; set; }
    }
    public class ItemResourceContent
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Priority { get; set; }
    }
    public class ItemResourceFile
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public int Priority { get; set; }
    }

    public class StandResourceCoordViewModel : CoordStandModel
    {
        public string ResourceTypeId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
