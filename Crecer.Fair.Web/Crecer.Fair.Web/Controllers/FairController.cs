using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Web.Models;
using Crecer.Fair.Web.Services;
using Crecer.Fair.Web.Services.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Crecer.Fair.Web.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIdConnectDefaults.AuthenticationScheme)]
    public class FairController : Controller
    {
        private IFairService fairService;
        private IProviderService providerService;
        private FairViewModelDTO fairModel;
        private IStandModelService standModelService;
        private string backOfficeUrl = "BackOfficeUrl";
        private string[] dayWeek = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

        public FairController(
                IFairService fairService,
                IProviderService providerService,
                IStandModelService standModelService,
                IConfiguration configuration)
        {
            this.fairService = fairService;
            this.providerService = providerService;
            this.standModelService = standModelService;

            InitFair();
            this.backOfficeUrl = configuration.GetValue<string>("FairSettings:BackOfficeUrl");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            StandIndexViewModel model = new StandIndexViewModel()
            {
                FairId = Fair.FairId
            };

            model.Stands.AddRange(await GetStandsAsync(Fair.FairId));

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Start()
        {
            Guid? providerStandId = null;

            if (this.User.GetFairProviderId().HasValue)
            {
                if (fairModel != null)
                {
                    var stands = await fairService.GetStandsAsync(fairModel.FairId);
                    var providerStand = stands.Where(p => p.ProviderId == this.User.GetFairProviderId().Value).OrderBy(p => p.PositionRef);
                    if (providerStand.Any())
                        providerStandId = providerStand.First().StandId;
                }
            }
            else if (fairModel != null)
                providerStandId = fairModel.StandId;

            if (providerStandId.HasValue)
                return RedirectToAction("Stand", "Fair", new { id = providerStandId });
            else
                return RedirectToAction("Index", "Fair");
        }

        [HttpGet]
        public IActionResult Admin()
        {
            return Redirect(backOfficeUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Stand(Guid id)
        {
            StandViewModel model = new StandViewModel();

            var stand = await fairService.GetStandByIdAsync(Fair.FairId, id);
            if (stand == null)
                return RedirectToAction(nameof(Index));

            var provider = await providerService.GetAsync(stand.ProviderId.Value);

            if (provider == null)
                return RedirectToAction(nameof(Index));

            //Register Visit
            await this.fairService.RegisterStandVisit(stand.FairId, stand.StandId);

            var resources = await providerService.GetResourcesAsync(provider.ProviderId);
            var calendar = await providerService.GetCalendarAsync(provider.ProviderId);

            model.ProviderId = provider.ProviderId;
            model.ProviderName = provider.Tradename;
            model.ContactData.Address = provider.Address;
            model.ContactData.Email = provider.Email;
            model.ContactData.FacebookAddress = provider.FacebookAddress;
            model.ContactData.InstagramAddress = provider.InstagramAddress;
            model.ContactData.PhoneNumber1 = provider.PhoneNumber1;
            model.ContactData.PhoneNumber2 = provider.PhoneNumber2;
            model.ContactData.TwitterAddress = provider.TwitterAddress;
            model.ContactData.WebSite = provider.Website;
            model.ContactData.YoutubeAddress = provider.YoutubeAddress;

            #region Logo

            var resourceLogo = resources.FirstOrDefault(p => p.ResourceTypeId == ResourceTypes.Logo);

            if (resourceLogo != null)
                model.LogoUrl = GetResourceUrl(resourceLogo.Value);

            #endregion

            #region Photos
            var resourcePhotos = resources.Where(p => p.ResourceTypeId == ResourceTypes.PhotoGallery).
                    Select(p => new ItemResourcePhoto()
                    {
                        Name = p.Name,
                        Priority = p.Priority,
                        ThumbnailUrl = GetResourceUrl(p.ThumbnailPath),
                        Url = GetResourceUrl(p.Value)
                    });

            if (resourcePhotos.Any())
                model.Photos.AddRange(resourcePhotos);

            #endregion

            #region Videos

            var resourceVideos = resources.Where(p => p.ResourceTypeId == ResourceTypes.Video).
                    Select(p => new ItemResourceFile()
                    {
                        Name = p.Name,
                        Priority = p.Priority,
                        MimeType = Ecubytes.Image.MimeTypeUtility.GetMimeType(System.IO.Path.GetExtension(p.FileName)),
                        Url = GetResourceUrl(p.Value)
                    });

            if (resourceVideos.Any())
                model.Videos.AddRange(resourceVideos);

            if (calendar.Any())
                model.CalendarItems = calendar.Select(p => new ItemResourceCalendar()
                {
                    End = p.End,
                    Start = p.Start,
                    WeekDay = p.WeekDay,
                    WeekDayDescription = dayWeek[p.WeekDay - 1]
                }).ToList();

            #endregion

            #region Documents

            var resourceDocuments = resources.Where(p => p.ResourceTypeId == ResourceTypes.Documents).
                    Select(p => new ItemResourceFile()
                    {
                        Name = p.Name,
                        Priority = p.Priority,
                        MimeType = Ecubytes.Image.MimeTypeUtility.GetMimeType(System.IO.Path.GetExtension(p.FileName)),
                        Url = GetResourceUrl(p.Value)
                    });

            if (resourceDocuments.Any())
                model.Files.AddRange(resourceDocuments);

            #endregion

            #region Links

            var resourceLinks = resources.Where(p => p.ResourceTypeId == ResourceTypes.Link).
                    Select(p => new ItemResourceLink()
                    {
                        Name = p.Name,
                        Url = p.Value,
                        Priority = p.Priority
                    });

            if (resourceLinks.Any())
                model.Links.AddRange(resourceLinks);

            #endregion

            #region Banners

            var resourceBanners = resources.Where(p => p.ResourceTypeId == ResourceTypes.Banner).
                    Select(p => new ItemResourceFile()
                    {
                        Name = p.Name,
                        Priority = p.Priority,
                        MimeType = Ecubytes.Image.MimeTypeUtility.GetMimeType(System.IO.Path.GetExtension(p.FileName)),
                        Url = GetResourceUrl(p.Value)
                    });

            if (resourceBanners.Any())
                model.Banner.AddRange(resourceBanners);

            #endregion

            #region TextContent

            var resourcesText = resources.Where(p => p.ResourceTypeId == ResourceTypes.Text).
                    Select(p => new ItemResourceContent()
                    {
                        Title = p.Name,
                        Content = p.Value,
                        Priority = p.Priority
                    });

            if (resourcesText.Any())
                model.Contents.AddRange(resourcesText);

            #endregion

            var standModel = await standModelService.GetAsync(stand.ModelCode);

            model.Coords.AddRange(standModel.Resources.Select(p => new StandResourceCoordViewModel()
            {
                StandModel = model,
                Height = p.Height,
                ResourceTypeId = p.ResourceTypeId,
                Width = p.Width,
                X = p.X,
                Y = p.Y
            }));

            model.ModelCode = standModel.ModelCode;

            return View(model);
        }

        #region methods

        private void InitFair()
        {
            fairModel = this.fairService.GetMainAsync().Result;
        }
        protected FairViewModelDTO Fair
        {
            get
            {
                return fairModel;
            }
        }

        private string GetResourceUrl(string relativeResource)
        {
            if (string.IsNullOrWhiteSpace(relativeResource))
                return null;

            return $"{backOfficeUrl}/{relativeResource}";
        }
        private async Task<List<StandCoordViewModel>> GetStandsAsync(Guid fairId)
        {
            var stands = await this.fairService.GetStandsAsync(fairId);

            return stands.Where(p => p.ProviderId.HasValue).Select(p => new StandCoordViewModel()
            {
                Height = p.Height,
                LogoUrl = GetResourceUrl(p.LogoUrl),
                ProviderId = p.ProviderId,
                ProviderName = p.ProviderName,
                StandId = p.StandId,
                Width = p.Width,
                X = p.X,
                Y = p.Y
            }).ToList();
        }

        #endregion

    }
}