using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Models;
using Crecer.Fair.Admin.Web.Services;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.AspNetCore.Mvc.DataTables;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Data.Common;
using Ecubytes.Extensions.AspNetCore.Mvc.DataTable.Models;
using Ecubytes.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class ProviderController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly ApiProfileManager apiProfileManager;
        private readonly IProviderService providerService;
        private IProviderCategoryService providerCategoryService;
        string[] dayWeek = { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };
        public ProviderController(
            IProviderService providerService,
            IProviderCategoryService providerCategoryService,
            ApiProfileManager apiProfileManager)
        {
            this.providerService = providerService;
            this.providerCategoryService = providerCategoryService;
            this.apiProfileManager = apiProfileManager;
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public IActionResult Index()
        {
            if (this.User.GetFairProviderId().HasValue && !this.User.HasAccessAll())
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> GetList([FromQuery] IDataTablesRequest request, bool export = false, int? totalCount = null)
        {
            var queryRequest = request.ToQueryRequest();

            var response = await this.providerService.GetAsync(queryRequest);

            if(!export)
            {
                return this.DataTableJsonResult(new DataTableResponseModel()
                {
                    Data = response.Data,
                    Page = response.Page.GetValueOrDefault(0),
                    TotalRecords = response.TotalCount,
                    TotalRecordsFiltered = response.TotalCount,
                    Request = request
                });
            }
            else
            {
                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    //workSheet.Cells.LoadFromCollection(response.Data, true, OfficeOpenXml.Table.TableStyles.None, System.Reflection.BindingFlags.Default,
                    workSheet.Cells[1, 1].Value = "RUC";
                    workSheet.Cells[1, 2].Value = "Nombre";
                    workSheet.Cells[1, 3].Value = "Email";
                    workSheet.Cells[1, 4].Value = "Dirección";
                    workSheet.Cells[1, 5].Value = "Teléfono";
                    workSheet.Cells[1, 6].Value = "Teléfono 2";
                    workSheet.Cells[1, 7].Value = "Activo";
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in response.Data)
                    {
                        workSheet.Cells[row, 1].Value = item.Identification;
                        workSheet.Cells[row, 2].Value = item.Tradename;
                        workSheet.Cells[row, 3].Value = item.Email;
                        workSheet.Cells[row, 4].Value = item.Address;
                        workSheet.Cells[row, 5].Value = item.PhoneNumber1;
                        workSheet.Cells[row, 6].Value = item.PhoneNumber2;
                        workSheet.Cells[row, 7].Value = item.Active ? "SI" : "NO";
                        row++;
                    }

                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    workSheet.Column(6).AutoFit();
                    workSheet.Column(7).AutoFit();

                    package.Save();
                }

                stream.Position = 0;
                string excelName = $"Proveedores-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> Create()
        {
            if (this.User.GetFairProviderId().HasValue && !this.User.HasAccessAll())
                return RedirectToAction("AccessDenied", "Account");

            ProviderEditViewModel model = new ProviderEditViewModel();
            model.Active = true;
            await SetCategoryListAsync(model);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!CanEdit(id))
                return RedirectToAction("AccessDenied", "Account");

            var data = await this.providerService.GetAsync(id);

            ProviderEditViewModel model = new ProviderEditViewModel();
            model.Active = data.Active;
            model.Address = data.Address;
            model.BusinessName = data.BusinessName;
            model.Email = data.Email;
            model.Identification = data.Identification;
            model.PhoneNumber1 = data.PhoneNumber1;
            model.PhoneNumber2 = data.PhoneNumber2;
            model.ProviderId = data.ProviderId;
            model.Tradename = data.Tradename;
            model.Website = data.Website;
            model.FacebookAddress = data.FacebookAddress;
            model.TwitterAddress = data.TwitterAddress;
            model.YoutubeAddress = data.YoutubeAddress;
            model.InstagramAddress = data.InstagramAddress;
            model.CategoryId = data.CategoryId;

            await SetCategoryListAsync(model);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> Create(ProviderEditViewModel requestModel)
        {
            if (!CanEdit(requestModel.ProviderId))
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                ProviderInsertRequestDTO model = new ProviderInsertRequestDTO();
                model.Active = requestModel.Active;
                model.Address = requestModel.Address;
                model.BusinessName = requestModel.BusinessName;
                model.Email = requestModel.Email;
                model.Identification = requestModel.Identification;
                model.PhoneNumber1 = requestModel.PhoneNumber1;
                model.PhoneNumber2 = requestModel.PhoneNumber2;
                model.Tradename = requestModel.Tradename;
                model.FacebookAddress = requestModel.FacebookAddress;
                model.TwitterAddress = requestModel.TwitterAddress;
                model.YoutubeAddress = requestModel.YoutubeAddress;
                model.InstagramAddress = requestModel.InstagramAddress;
                model.CategoryId = requestModel.CategoryId;

                var response = await this.providerService.InsertAsync(model);

                if (response.IsValid)
                    return RedirectToAction(nameof(Index));
                else
                    this.AddMessages(response);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return View(requestModel);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> Edit(ProviderEditViewModel requestModel)
        {
            if (!CanEdit(requestModel.ProviderId))
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                ProviderUpdateRequestDTO model = new ProviderUpdateRequestDTO();
                model.ProviderId = requestModel.ProviderId;
                model.Active = requestModel.Active;
                model.Address = requestModel.Address;
                model.BusinessName = requestModel.BusinessName;
                model.Email = requestModel.Email;
                model.Website = requestModel.Website;
                model.Identification = requestModel.Identification;
                model.PhoneNumber1 = requestModel.PhoneNumber1;
                model.PhoneNumber2 = requestModel.PhoneNumber2;
                model.Tradename = requestModel.Tradename;
                model.FacebookAddress = requestModel.FacebookAddress;
                model.TwitterAddress = requestModel.TwitterAddress;
                model.YoutubeAddress = requestModel.YoutubeAddress;
                model.InstagramAddress = requestModel.InstagramAddress;
                model.CategoryId = requestModel.CategoryId;

                var response = await this.providerService.UpdateAsync(model);

                if (response.IsValid)
                    return RedirectToAction(nameof(Index));
                else
                    this.AddMessages(response);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return View(requestModel);
        }


        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditUsers(Guid id)
        {
            if (!CanEdit(id))
                return RedirectToAction("AccessDenied", "Account");

            var data = await this.providerService.GetAsync(id);

            ProviderEditUsersViewModel viewModel = new ProviderEditUsersViewModel();
            viewModel.Identification = data.Identification;
            viewModel.ProviderId = data.ProviderId;
            viewModel.Tradename = data.Tradename;

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> GetUsers(Guid providerId)
        {
            if (!CanEdit(providerId))
                return RedirectToAction("AccessDenied", "Account");

            var relationData = await this.providerService.GetUsersAsync(providerId);

            List<ProviderUserViewModel> viewModel = new List<ProviderUserViewModel>();

            if (relationData != null && relationData.Any())
            {
                var queryRequest = QueryRequest.Builder();

                queryRequest.AddCondition("UserId", relationData.Select(p => p.UserId).ToArray());

                IdentityClient identityClient = GetIdentityClient();
                var users = await identityClient.GetAsync(queryRequest);

                viewModel = users.Data.Select(p => new ProviderUserViewModel()
                {
                    ProviderId = providerId,
                    UserFullName = p.FullName,
                    UserId = p.UserId,
                    UserLogonName = p.LogonName
                }).ToList();
            }

            return Json(new { data = viewModel });
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> AddUser(Guid providerId, Guid userId)
        {
            if (!CanEdit(providerId))
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                var response = await providerService.AddUsersAsync(providerId, userId);
                this.AddMessages(response);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return JsonModelResult();
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> RemoveUser(Guid providerId, Guid userId)
        {
            if (!CanEdit(providerId))
                return RedirectToAction("AccessDenied", "Account");

            try
            {
                var response = await providerService.RemoveUsersAsync(providerId, userId);
                this.AddMessages(response);
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return JsonModelResult();
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditCalendar(Guid id)
        {
            if (!CanEdit(id))
                return RedirectToAction("AccessDenied", "Account");

            var data = await this.providerService.GetAsync(id);
            var currentCalendar = await this.providerService.GetCalendarAsync(id);

            ProviderCalendarEditViewModel viewModel = new ProviderCalendarEditViewModel();
            viewModel.Identification = data.Identification;
            viewModel.ProviderId = data.ProviderId;
            viewModel.Tradename = data.Tradename;

            viewModel.CalendarItems = new List<ProviderCalendarItemEditViewModel>();

            foreach (var item in currentCalendar)
            {
                viewModel.CalendarItems.Add(new ProviderCalendarItemEditViewModel()
                {
                    WeekDay = item.WeekDay,
                    Start = item.Start,
                    End = item.End
                });
            }

            for (int i = 1; i <= dayWeek.Count(); i++)
            {
                string name = dayWeek[i - 1];
                var d = viewModel.CalendarItems.FirstOrDefault(p => p.WeekDay == i);
                if (d == null)
                {
                    d = new ProviderCalendarItemEditViewModel()
                    {
                        WeekDayDescription = name,
                        WeekDay = i
                    };

                    viewModel.CalendarItems.Add(d);
                }
                else
                    d.WeekDayDescription = name;
            }


            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.ProviderEditor)]
        public async Task<IActionResult> EditCalendar(ProviderCalendarEditViewModel model)
        {
            try
            {
                if (model.CalendarItems == null)
                    model.CalendarItems = new List<ProviderCalendarItemEditViewModel>();

                if (model.CalendarItems.Any(p =>
                     string.IsNullOrWhiteSpace(p.Start) || string.IsNullOrWhiteSpace(p.End)))
                    this.AddErrorMessage("Los campos Desde y Hasta son requeridos para los días aplicables");

                if (this.IsValidState)
                {
                    DateTime startOut = DateTime.Now;
                    DateTime endOut = DateTime.Now;

                    if (model.CalendarItems.Any(p =>
                        !DateTime.TryParseExact(p.Start, "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startOut) ||
                        !DateTime.TryParseExact(p.End, "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endOut) ))
                        this.AddErrorMessage("Existe uno o varios horarios con valores inválidos");
                }

                if (this.IsValidState)
                {
                    if (model.CalendarItems.Any(p => DateTime.ParseExact(p.End, "HH:mm", System.Globalization.CultureInfo.InvariantCulture)
                         < DateTime.ParseExact(p.Start, "HH:mm", System.Globalization.CultureInfo.InvariantCulture)))
                        this.AddErrorMessage("La hora Desde debe ser menor a la hora Hasta");
                }

                if (this.IsValidState)
                {
                    var response = await providerService.SetCalendarAsync(model.ProviderId,
                        model.CalendarItems.Select(p => new ProviderCalendarInsertRequestDTO()
                        {
                            WeekDay = p.WeekDay,
                            End = p.End,
                            Start = p.Start
                        })
                        );

                    this.AddMessages(response);
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return JsonModelResult(@Url.Action("Index", "Provider"));
        }

        #region methods        

        private IdentityClient GetIdentityClient()
        {
            var profile = apiProfileManager.Get("Identity.User");

            return new IdentityClient(
                profile.BaseAddress,
                profile.ClientId,
                profile.ClientSecret,
                GlobalOptions.DefaultTenantId);
        }

        private async Task SetCategoryListAsync(ProviderEditViewModel model)
        {
            var categories = await providerCategoryService.GetAsync(new Ecubytes.Data.Common.QueryRequest());

            model.CategorySelectList = new SelectList(categories.Data, "CategoryId", "Name");
        }

        private bool CanEdit(Guid providerId)
        {
            return User.HasAccessAll() || !this.User.GetFairProviderId().HasValue || this.User.GetFairProviderId() == providerId;
        }

        #endregion
    }
}