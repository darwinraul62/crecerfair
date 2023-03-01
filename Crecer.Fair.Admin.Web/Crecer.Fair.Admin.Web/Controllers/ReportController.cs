using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using OfficeOpenXml;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class ReportController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IFairService fairService;
        private readonly ApiProfileManager apiProfileManager;
        private IdentityClient identityClient;
        public ReportController(IFairService fairService, ApiProfileManager apiProfileManager)
        {
            this.fairService = fairService;
            this.apiProfileManager = apiProfileManager;
            InitIdentityClient();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLoginCount(DateTime dateFrom, DateTime dateTo)
        {
            dateFrom = dateFrom.ToLocalTime().Date.ToUniversalTime();
            dateTo = dateTo.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();

            var viewModel = await identityClient.GetLoginCountAsync(new Ecubytes.Identity.Models.LoginCountRequest()
            {
                DateFrom = dateFrom,
                DateTo = dateTo
            });

            return Json(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserOnlineCount()
        {
            var viewModel = await identityClient.GetUserOnlineCountAsync();

            return Json(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStandsCount()
        {
            var fairModel = await this.fairService.GetMainAsync();
            var viewModel = await fairService.GetStandsCountAsync(fairModel.FairId);

            return Json(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStandsVisitCount(DateTime dateFrom, DateTime dateTo)
        {
            var fairModel = await this.fairService.GetMainAsync();

            dateFrom = dateFrom.ToLocalTime().Date.ToUniversalTime();
            dateTo = dateTo.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();

            var viewModel = await fairService.GetStandsVisitCountAsync(fairModel.FairId,
                dateFrom,
                dateTo,
                User.GetFairProviderId()
                );

            return Json(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserRegisterCount(DateTime dateFrom, DateTime dateTo)
        {
            var fairModel = await this.fairService.GetMainAsync();

            dateFrom = dateFrom.ToLocalTime().Date.ToUniversalTime();
            dateTo = dateTo.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();

            var viewModel = await identityClient.GetRegisterCountAsync(new Ecubytes.Identity.Models.UserRegisterCountRequest()
            {
                DateFrom = dateFrom,
                DateTo = dateTo
            });

            return Json(viewModel);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProviderVisitCount(DateTime dateFrom, DateTime dateTo, [FromQuery] IDataTablesRequest dtRequest,
            bool export = false, int? totalCount = null)
        {
            var fairModel = await this.fairService.GetMainAsync();

            dateFrom = dateFrom.ToLocalTime().Date.ToUniversalTime();
            dateTo = dateTo.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();

            var viewModel = await fairService.GetProviderVisitCountAsync(fairModel.FairId,
                    dateFrom,
                    dateTo,
                    User.GetFairProviderId()
                    );

            if (!export)
            {
                return Json(new { data = viewModel });
            }
            else
            {
                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    //workSheet.Cells.LoadFromCollection(response.Data, true, OfficeOpenXml.Table.TableStyles.None, System.Reflection.BindingFlags.Default,
                    workSheet.Cells[1, 1].Value = "Proveedor";
                    workSheet.Cells[1, 2].Value = "Visitas";

                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in viewModel)
                    {
                        workSheet.Cells[row, 1].Value = item.Provider;
                        workSheet.Cells[row, 2].Value = item.VisitCount;
                        row++;
                    }

                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();

                    package.Save();
                }

                stream.Position = 0;
                string excelName = $"StandsVisitados-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }


        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.ReportRead)]
        public IActionResult PartnertStandVisited()
        {
            return View();
        }


        [HttpGet]
        [Authorize(Roles = SecurityRoles.ReportRead)]
        public async Task<IActionResult> GetPartnertStandVisit(DateTime dateFrom, DateTime dateTo, [FromQuery] IDataTablesRequest dtRequest,
            bool export = false, int? totalCount = null)
        {
            dateFrom = dateFrom.ToLocalTime().Date.ToUniversalTime();
            dateTo = dateTo.ToLocalTime().Date.AddDays(1).AddSeconds(-1).ToUniversalTime();

            var queryRequest = dtRequest.ToQueryRequest();

            var fairModel = await this.fairService.GetMainAsync();

            var response = await this.fairService.GetPartnertStandVisitCountAsync(fairModel.FairId, dateFrom, dateTo, queryRequest);

            if (!export)
            {
                return this.DataTableJsonResult(new DataTableResponseModel()
                {
                    Data = response.Data,
                    Page = response.Page.GetValueOrDefault(0),
                    TotalRecords = response.TotalCount,
                    TotalRecordsFiltered = response.TotalCount,
                    Request = dtRequest
                });
            }
            else
            {
                var stream = new MemoryStream();

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    //workSheet.Cells.LoadFromCollection(response.Data, true, OfficeOpenXml.Table.TableStyles.None, System.Reflection.BindingFlags.Default,
                    workSheet.Cells[1, 1].Value = "Socio";
                    workSheet.Cells[1, 2].Value = "Identificación";
                    workSheet.Cells[1, 3].Value = "Email";
                    workSheet.Cells[1, 4].Value = "Teléfono";
                    workSheet.Cells[1, 5].Value = "Stand";
                    workSheet.Cells[1, 6].Value = "Visitas";
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in response.Data)
                    {
                        workSheet.Cells[row, 1].Value = item.Partnert;
                        workSheet.Cells[row, 2].Value = item.PartnertIdentification;
                        workSheet.Cells[row, 3].Value = item.PartnertEmail;
                        workSheet.Cells[row, 4].Value = item.PartnertPhoneNumber;
                        workSheet.Cells[row, 5].Value = item.Provider;
                        workSheet.Cells[row, 6].Value = item.VisitCount;
                        row++;
                    }

                    workSheet.Column(1).AutoFit();
                    workSheet.Column(2).AutoFit();
                    workSheet.Column(3).AutoFit();
                    workSheet.Column(4).AutoFit();
                    workSheet.Column(5).AutoFit();
                    workSheet.Column(6).AutoFit();

                    package.Save();
                }

                stream.Position = 0;
                string excelName = $"SociosStands-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
        }

        public async Task<IActionResult> GetOnlineUsers()
        {
            var usersOnline = await identityClient.GetAsync(QueryRequest.Builder().AddCondition("Online", true));

            var filterUserId = QueryRequest.Builder().AddCondition("UserId", usersOnline.Data.Select(p => p.UserId).ToArray());

            var fairUsers = await this.fairService.GetFairsUsersAsync(filterUserId);

            List<UserReport> data = new List<UserReport>();

            // data.AddRange(fairUsers.Data.Select(p=> new UserReport()
            // {
            //     Email = p.Email,
            //     FullName = p.FullName,
            //     IsPartnert = p.IsPartnert,
            //     IsProvider = p.IsProvider,
            //     PhoneNumber = p.PhoneNumber,
            //     ProviderName = p.FullName,
            //     UserId = p.UserId
            // }));

            data.AddRange(usersOnline.Data.
                Select(p => new UserReport()
                {
                    Email = p.Email,
                    FullName = p.FullName,
                    IsPartnert = false,
                    IsProvider = false,
                    UserId = p.UserId,
                    BlockLogin = p.BlockLogin,
                    LogonName = p.LogonName,
                    Online = p.Online,
                    LastAccess = p.LastAccess
                }));

            foreach (var p in fairUsers.Data)
            {
                var user = data.FirstOrDefault(p => p.UserId == p.UserId);
                if (user != null)
                {
                    user.IsPartnert = p.IsPartnert;
                    user.IsProvider = p.IsProvider;
                    if (user.IsProvider)
                        user.ProviderName = p.FullName;
                }
            }

            return Json(new { data = data });
        }


        private void InitIdentityClient()
        {
            var profile = apiProfileManager.Get("Identity.User");

            identityClient = new IdentityClient(
                profile.BaseAddress,
                profile.ClientId,
                profile.ClientSecret,
                GlobalOptions.DefaultTenantId);
        }
    }
}
