using System;

namespace Crecer.Fair.Data
{
    public class ResourceType
    {
        public string ResourceTypeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool UserCanCreate { get; set; }
        public int Priority { get; set; }
    }

    public static class ResourceTypes
    {
        public const string Text = "TEXT";
        public const string Link = "LINK";
        public const string Logo = "LOGO";
        public const string PhotoGallery = "PHOTOGALLERY";
        public const string Banner = "BANNER"; 
        public const string Documents = "DOCUMENTS";
        public const string Video = "VIDEO";
        public const string Calendar = "CALENDAR";
        public const string SocialNetworks = "SOCIALNETWORKS";
        public static string[] GeneralResources = new string[]
        {
            Text, Link
        };
    }
}
