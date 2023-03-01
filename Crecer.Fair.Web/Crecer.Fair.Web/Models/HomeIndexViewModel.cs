using System;
using System.Collections.Generic;
using Crecer.Fair.Web.Models;

namespace Crecer.Fair.Web.Models
{
    public class HomeIndexViewModel
    {
        public FairViewModel Fair { get; set; }

        public List<FairBannerUrl> BannerItems { get; set; }
    }
    public class FairBannerUrl
    {
        public string Url { get; set; }
    }
}
