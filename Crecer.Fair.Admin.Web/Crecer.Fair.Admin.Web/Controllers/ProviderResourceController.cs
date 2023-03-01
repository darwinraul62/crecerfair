using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Models;
using Crecer.Fair.Admin.Web.Services;
using Crecer.Fair.Admin.Web.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class ProviderResourceController : FairBaseController
    {
        private IProviderService providerService;
        private IResourceTypeService resourceTypeService;
        private IWebHostEnvironment hostEnvironment;
        private IConfiguration configuration;
        public ProviderResourceController(IProviderService providerService, IFairService fairService, IResourceTypeService resourceTypeService,
            IWebHostEnvironment hostEnvironment,
            IConfiguration configuration)
            : base(fairService)
        {
            this.providerService = providerService;
            this.resourceTypeService = resourceTypeService;
            this.configuration = configuration;
            this.hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> Index(Guid id)
        {
            ProviderResourceIndexViewModel model = new ProviderResourceIndexViewModel();

            var provider = await providerService.GetAsync(id);
            if (provider == null)
                return NotFound();

            model.ProviderId = provider.ProviderId;
            model.ProviderName = provider.BusinessName;

            var resourceList = await providerService.GetResourcesAsync(provider.ProviderId);
            var resourceTypes = (await resourceTypeService.GetAsync()).ToList();

            resourceTypes.RemoveAll(p => p.ResourceTypeId == ResourceTypes.Contact);
            if (!(Fair.HostProviderId.HasValue && Fair.HostProviderId == model.ProviderId))
            {
                resourceTypes.RemoveAll(p => p.ResourceTypeId == ResourceTypes.FairBanner);
            }

            model.ResourceTypes = resourceTypes.OrderBy(p => p.Priority).Select(
                p => new ResourceTypeViewModel()
                {
                    Active = p.Active,
                    Name = p.Name,
                    Priority = p.Priority,
                    ResourceTypeId = p.ResourceTypeId,
                    UserCanCreate = p.UserCanCreate,
                    MainData = model
                }
            ).ToList();

            foreach (var type in model.ResourceTypes)
            {
                type.Resources = resourceList.Where(p => p.ResourceTypeId == type.ResourceTypeId).Select(p => new ProviderResourceViewModel()
                {
                    Name = p.Name,
                    ResourceId = p.ResourceId,
                    ResourceTypeId = p.ResourceTypeId,
                    ResourceTypeName = p.ResourceTypeName,
                    Value = p.Value,
                    Priority = p.Priority,
                    ThumbnailPath = p.ThumbnailPath,
                    FileName = p.FileName
                }).ToList();

                if (!type.UserCanCreate && (type.Resources == null || !type.Resources.Any())
                    &&
                    (type.ResourceTypeId != ResourceTypes.PhotoGallery
                        && type.ResourceTypeId != ResourceTypes.Banner
                        && type.ResourceTypeId != ResourceTypes.FairBanner
                        && type.ResourceTypeId != ResourceTypes.Documents
                        && type.ResourceTypeId != ResourceTypes.Video))
                {
                    type.Resources.Add(new ProviderResourceViewModel()
                    {
                        ResourceTypeName = type.Name,
                    });
                }
            }

            // foreach (var resourceType in resourceTypes.Where(p => !p.UserCanCreate
            //      && !model.res.Any(r => r.ResourceTypeId == p.ResourceTypeId)).OrderBy(p => p.Priority))
            // {
            //     model.resourceType.Add(new ProviderResourceViewModel()
            //     {
            //         Name = resourceType.Name,
            //         ResourceTypeId = resourceType.ResourceTypeId,
            //         ResourceTypeName = resourceType.Name,
            //         Priority = resourceType.Priority
            //     });
            // }

            return View(model);
        }

        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public IActionResult Edit(Guid providerId, string type, Guid? resourceId, bool delete = false)
        {
            string actionName = null;

            switch (type)
            {
                case ResourceTypes.Logo:
                    actionName = nameof(EditLogo);
                    break;
                case ResourceTypes.PhotoGallery:
                case ResourceTypes.Documents:
                case ResourceTypes.Banner:
                case ResourceTypes.FairBanner:
                case ResourceTypes.Video:
                    actionName = nameof(EditFiles);
                    break;
                default:
                    actionName = nameof(EditGeneral);
                    break;

            }

            return RedirectToAction(actionName, new { providerId = providerId, type = type, resourceId = resourceId, delete = delete });
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditGeneral(Guid providerId, string type, Guid? resourceId, bool delete = false)
        {

            if (type != ResourceTypes.Text && type != ResourceTypes.Link)
            {
                this.AddErrorMessage("Tipo de Recurso no permitido");
                return RedirectToIndex(providerId);
            }

            ProviderResourceGeneralEditViewModel model = await PrepareModelAsync(resourceId, type, providerId);

            if (!this.IsValidState)
                return RedirectToIndex(providerId);

            string viewName = string.Empty;

            if (model.ResourceTypeId == ResourceTypes.Text)
                viewName = "Edit_Editor";
            else
                viewName = "Edit_Link";

            if (delete)
            {
                model.IsReadOnly = true;
                model.IsDelete = true;
            }

            return View(viewName, model);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditGeneral(ProviderResourceGeneralEditViewModel requestModel)
        {
            try
            {
                ProviderResourceGeneralUpdateRequestDTO model = new ProviderResourceGeneralUpdateRequestDTO();
                model.Name = requestModel.Name;
                model.ResourceId = requestModel.ResourceId;
                model.ResourceTypeId = requestModel.ResourceTypeId;
                model.Value = requestModel.Value;
                model.Priority = requestModel.Priority;

                var response = await providerService.AddResourceAsync(requestModel.ProviderId, model);

                if (!response.IsValid)
                    this.AddMessages(response);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return JsonModelResult(this.Url.Action(nameof(Index), new { id = requestModel.ProviderId }));
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditLogo(Guid providerId, string type, Guid? resourceId)
        {
            if (type != ResourceTypes.Logo)
            {
                this.AddErrorMessage("Tipo de Recurso no permitido");
                return RedirectToIndex(providerId);
            }

            ProviderResourceGeneralEditViewModel model = await PrepareModelAsync(resourceId, type, providerId);

            if (!this.IsValidState)
                return RedirectToIndex(providerId);

            return View("Edit_Logo", model);
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditFiles(Guid providerId, string type, Guid? resourceId)
        {
            if (type != ResourceTypes.PhotoGallery
                && type != ResourceTypes.Documents
                && type != ResourceTypes.Banner
                && type != ResourceTypes.FairBanner
                && type != ResourceTypes.Video)
            {
                this.AddErrorMessage("Tipo de Recurso no permitido");
                return RedirectToIndex(providerId);
            }

            ProviderResourceGeneralEditViewModel model = await PrepareModelAsync(resourceId, type, providerId);

            if (!this.IsValidState)
                return RedirectToIndex(providerId);

            return View("Edit_FileUpload", model);
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> PartialEditFiles(Guid providerId, string type)
        {
            ProviderResourceGeneralEditViewModel model = await PrepareModelAsync(null, type, providerId);
            model.IsAjaxRequest = true;

            return PartialView("_Edit_FileUpload", model);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditResourceMedia(
           Guid providerId,
           Guid? resourceId,
           string resourceType,
           string fileName)
        {
            ProviderResourceViewModelDTO previousResource = null;
            try
            {
                if (!Request.Form.Files.Any())
                    return JsonModelResult();

                IFormFile blob = Request.Form.Files[0];

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = blob.FileName;
                }

                if (!resourceId.HasValue || resourceId.Value == Guid.Empty)
                {
                    resourceId = Guid.NewGuid();
                }
                else
                {
                    previousResource = await providerService.GetResourceAsync(providerId, resourceId.Value);
                }

                //string webRootPath = hostEnvironment.;
                //string contentRootPath = hostEnvironment.ContentRootPath;
                Guid guidFile = Guid.NewGuid();
                string fileNameResource = $"{guidFile}{Path.GetExtension(fileName)}";
                string fileNameMinResource = $"{guidFile}_min{Path.GetExtension(fileName)}";

                string simpleFileName = Path.GetFileName(fileName);

                //string path = Path.Combine(webRootPath, "files");
                string path = GetFilePath();
                string url = $"/files/{fileNameResource}";
                string urlMin = $"/files/{fileNameMinResource}";

                if (!Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                string filePath = Path.Combine(path, Path.GetFileName(fileNameResource));
                string filePathMin = Path.Combine(path, Path.GetFileName(fileNameMinResource));

                if (blob.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await blob.CopyToAsync(stream);
                        var minImage = GetReducedImage(150, 150, stream);
                        if (minImage != null)
                        {
                            minImage.Save(filePathMin);
                        }
                    }
                    Logger.LogDebug("File Temp Created");
                }

                try
                {
                    ProviderResourceGeneralUpdateRequestDTO model = new ProviderResourceGeneralUpdateRequestDTO();
                    model.Name = simpleFileName;
                    model.FileName = simpleFileName;
                    model.ResourceId = resourceId;
                    model.ResourceTypeId = resourceType;
                    model.ThumbnailPath = urlMin;
                    model.Value = url;

                    var response = await providerService.AddResourceAsync(providerId, model);

                    if (!response.IsValid)
                        this.AddMessages(response);

                    if (previousResource != null)
                    {
                        //Borrar el archivo anterior
                        try
                        {
                            DeleteResourceFile(previousResource);
                        }
                        catch (Exception ex)
                        {
                            this.Logger.LogError(ex, "Could not delete previous file");
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.AddDefaultErrorMessage(ex);
                    System.IO.File.Delete(filePath);
                    Logger.LogDebug("File Temp Deleted");
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
                return Json(new { IsValid = false, error = ex.Message, Messages = Messages });
            }

            return JsonModelResult(this.Url.Action(nameof(Index), new { id = providerId }));
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> DeleteResourceMedia(
           Guid providerId)
        {
            try
            {
                Guid resourceId = Guid.Parse(Request.Form["key"]);

                ProviderResourceViewModelDTO previousResource =
                    await providerService.GetResourceAsync(providerId, resourceId);

                if (previousResource != null)
                {
                    //Borrar el archivo anterior
                    DeleteResourceFile(previousResource);

                    await providerService.RemoveResourceAsync(providerId, resourceId);
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
                return Json(new { error = ex.Message });
            }
            return JsonModelResult();
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> DeleteResource(
           Guid providerId, Guid resourceId)
        {
            try
            {
                await providerService.RemoveResourceAsync(providerId, resourceId);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
                return Json(new { error = ex.Message });
            }
            return JsonModelResult(Url.Action(nameof(Index), new { id = providerId }));
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> UpdateResourceName(ProviderResourceGeneralEditViewModel model)
        {
            try
            {
                await this.providerService.UpdateResourceNameAsync(model.ProviderId, model.ResourceId, model.Name);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return JsonModelResult();
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> UpdateResourcesPriority(
            [FromQuery] Guid providerId, [FromQuery] string type,
            [FromBody] IEnumerable<ProviderResourceGeneralEditViewModel> files)
        {
            if (files != null && files.Any())
            {
                List<ProviderResourceUpdatePriorityDTO> models = files.Select(p => new ProviderResourceUpdatePriorityDTO()
                {
                    ResourceId = p.ResourceId,
                    Priority = p.Priority
                }).ToList();

                await providerService.UpdateResourcesPriorityAsync(providerId, type, models);
            }

            return JsonModelResult();
        }

        #region methods

        private async Task<ProviderResourceGeneralEditViewModel> PrepareModelAsync(Guid? resourceId, string type, Guid providerId)
        {
            ProviderResourceGeneralEditViewModel model = new ProviderResourceGeneralEditViewModel();

            //get collection resources if apply
            if (type == ResourceTypes.PhotoGallery
                || type == ResourceTypes.Documents
                || type == ResourceTypes.Banner
                || type == ResourceTypes.FairBanner
                || type == ResourceTypes.Video)
            {
                var resources = await providerService.GetResourcesAsync(providerId, type);

                if (resources.Any())
                {
                    model.ResourceCollection = resources.Select(p => new ProviderResourceValueViewModel()
                    {
                        Name = p.Name,
                        FileName = p.FileName,
                        Value = p.Value,
                        ResourceId = p.ResourceId,
                        Priority = p.Priority
                    }).ToList();

                    var resource = resources.FirstOrDefault();

                    model.ResourceTypeId = resource.ResourceTypeId;
                    model.ResourceTypeName = resource.ResourceTypeName;
                    model.Name = resource.Name;
                    model.Priority = resource.Priority;
                    model.Value = resource.Value;
                }
                else
                {
                    model.ResourceCollection = new List<ProviderResourceValueViewModel>();
                }
            }
            else if (resourceId.HasValue && resourceId.Value != Guid.Empty)
            {
                var resource = await providerService.GetResourceAsync(providerId, resourceId.Value);

                if (resource == null)
                {
                    this.AddErrorMessage("Recurso Inválido");
                    return model;
                }
                else
                {
                    model.ResourceId = resource.ResourceId;
                    model.ResourceTypeId = resource.ResourceTypeId;
                    model.ResourceTypeName = resource.ResourceTypeName;
                    model.Name = resource.Name;
                    model.Priority = resource.Priority;
                    model.Value = resource.Value;
                }
            }

            if (model.ResourceTypeName == null)
            {
                var resourceType = await resourceTypeService.GetAsync(type);
                model.ResourceTypeId = resourceType.ResourceTypeId;
                model.ResourceTypeName = resourceType.Name;
            }

            var provider = await providerService.GetAsync(providerId);

            if (provider != null)
            {
                model.ProviderId = provider.ProviderId;
                model.ProviderName = provider.Tradename;
            }
            else
            {
                this.AddErrorMessage("Código de Proveedor Inválido");
                return model;
            }

            return model;
        }

        private RedirectToActionResult RedirectToIndex(Guid providerId)
        {
            return RedirectToAction(nameof(Index), new { id = providerId });
        }

        private void DeleteResourceFile(ProviderResourceViewModelDTO deleteResource)
        {
            string webRootPath = hostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "files");

            string previousFilePath = null;
            string previousFileMinPath = null;

            if (!string.IsNullOrWhiteSpace(deleteResource.Value))
                previousFilePath = Path.Combine(path, Path.GetFileName(deleteResource.Value));

            if (!string.IsNullOrWhiteSpace(deleteResource.ThumbnailPath))
                previousFileMinPath = Path.Combine(path, Path.GetFileName(deleteResource.ThumbnailPath));

            if (!string.IsNullOrWhiteSpace(previousFilePath) && System.IO.File.Exists(previousFilePath))
                System.IO.File.Delete(previousFilePath);

            if (!string.IsNullOrWhiteSpace(previousFileMinPath) && System.IO.File.Exists(previousFileMinPath))
                System.IO.File.Delete(previousFileMinPath);

            Logger.LogDebug("Deleted Previous file {0}", previousFilePath);
        }

        private Image GetReducedImage(int maxWidth, int maxHeight, Stream resourceImage)
        {
            try
            {
                SizeF newSize;

                using (Bitmap srcBmp = new Bitmap(resourceImage))
                {
                    //double factor = (double)maxWidth / (double)srcBmp.Width;                    

                    var ratioX = (double)maxWidth / (double)srcBmp.Width;
                    var ratioY = (double)maxHeight / (double)srcBmp.Height;
                    var ratio = Math.Min(ratioX, ratioY);

                    //ratio = (double)srcBmp.Width / (double)srcBmp.Height;
                    newSize = new SizeF(Convert.ToInt32(srcBmp.Width * ratio), Convert.ToInt32(srcBmp.Height * ratio));
                }

                var image = Image.FromStream(resourceImage);
                var thumb = image.GetThumbnailImage((int)newSize.Width, (int)newSize.Height, () => false, IntPtr.Zero);

                return thumb;
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, e.Message);
                return null;
            }
        }

        private string GetFilePath()
        {
            string path = configuration.GetValue<string>("FairAdminSettings:FilesPath");
            if (!string.IsNullOrWhiteSpace(path))
                return path;
            else
                return Path.Combine(hostEnvironment.WebRootPath, "files");
        }

        #endregion
    }
}