using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Models;
using Crecer.Fair.Admin.Web.Services;
using Ecubytes.AspNetCore.Mvc.DataTables;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Data.Common;
using Ecubytes.Extensions.AspNetCore.Mvc.DataTable.Models;
using Ecubytes.Identity;
using Ecubytes.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class UserController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly ApiProfileManager apiProfileManager;
        private IFairService fairService;
        private IdentityClient identityClient;
        public UserController(ApiProfileManager apiProfileManager, IFairService fairService)
        {
            this.apiProfileManager = apiProfileManager;
            this.fairService = fairService;
            InitIdentityClient();
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> GetListUsers([FromQuery] IDataTablesRequest request, bool export = false, int? totalCount = null)
        {
            var queryRequest = request.ToQueryRequest();

            var response = await identityClient.GetAsync(queryRequest);

            List<UserReport> data = response.Data.Select(p => new UserReport()
            {
                BlockLogin = p.BlockLogin,
                Email = p.Email,
                FullName = p.FullName,
                IsPartnert = false,
                IsProvider = false,
                LastAccess = p.LastAccess,
                LogonName = p.LogonName,
                Online = p.Online,
                UserGroupNames = p.UserGroupNames,
                UserId = p.UserId,
                LastAccessString = p.LastAccess?.ToString("dd/MM/yyyy")
            }).ToList();

            var filterUserId = QueryRequest.Builder().AddCondition("UserId", data.Select(p => p.UserId).ToArray());

            var fairUsers = await this.fairService.GetFairsUsersAsync(filterUserId);

            foreach (var p in fairUsers.Data)
            {
                var user = data.FirstOrDefault(u => u.UserId == p.UserId);
                if (user != null)
                {
                    user.IsPartnert = p.IsPartnert;
                    user.IsProvider = p.IsProvider;
                    if (user.IsProvider)
                        user.ProviderName = p.FullName;
                }
            }

            if (!export)
            {
                return this.DataTableJsonResult(new DataTableResponseModel()
                {
                    Data = data,
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
                    workSheet.Cells[1, 1].Value = "Email";
                    workSheet.Cells[1, 2].Value = "Nombres";                    
                    workSheet.Cells[1, 3].Value = "Grupo de Usuario";
                    workSheet.Cells[1, 4].Value = "Socio";
                    workSheet.Cells[1, 5].Value = "Proveedor";
                    workSheet.Cells[1, 6].Value = "último Acceso";
                    workSheet.Cells[1, 7].Value = "Bloqueado";

                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in data)
                    {
                        workSheet.Cells[row, 1].Value = item.Email;
                        workSheet.Cells[row, 2].Value = item.FullName;
                        workSheet.Cells[row, 3].Value = item.UserGroupNames;
                        workSheet.Cells[row, 4].Value = item.IsPartnert ? "SI" : "NO";
                        workSheet.Cells[row, 5].Value = item.ProviderName;
                        workSheet.Cells[row, 6].Value = item.LastAccessString;
                        workSheet.Cells[row, 7].Value = item.BlockLogin ? "SI" : "NO";
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
                string excelName = $"Usuarios-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public IActionResult Create()
        {
            UserCreateViewModel viewModel = new UserCreateViewModel();
            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await identityClient.GetAsync(id);

            UserEditViewModel viewModel = new UserEditViewModel()
            {
                BlockLogin = model.BlockLogin,
                Email = model.Email,
                LastNames = model.LastNames,
                Names = model.Names,
                UserId = model.UserId
            };

            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> EditUserGroup(Guid id)
        {
            var model = await identityClient.GetAsync(id);

            var userGroup = (await identityClient.GetUserGroupForUserAsync(model.UserId)).FirstOrDefault();

            UserEditGroupViewModel viewModel = new UserEditGroupViewModel()
            {
                Email = model.Email,
                FullName = $"{model.Names} {model.LastNames}",
                UserId = model.UserId,
                UserGroupId = userGroup?.UserGroupId,
                UserGroupName = userGroup?.Name
            };

            await PrepareEditUserGroupModel(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> EditUserGroup(UserEditGroupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await identityClient.RemoveAllUserGroupForUser(viewModel.UserId);
                if (viewModel.UserGroupId.HasValue)
                {
                    await identityClient.AddUserGroupForUser(new UserGroupAddToUserRequest()
                    {
                        UserGroupId = viewModel.UserGroupId.Value,
                        UserId = viewModel.UserId
                    });
                }

                return RedirectToAction(nameof(Index));
            }

            await PrepareEditUserGroupModel(viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> Create(UserCreateViewModel viewModel)
        {
            try
            {
                if (viewModel.Password != viewModel.ConfirmPassword)
                {
                    this.ModelState.AddModelError("ConfirmPassword", "Las Contraseñas no coinciden");
                }

                if (ModelState.IsValid)
                {
                    UserCreateRequest requestModel = new UserCreateRequest()
                    {
                        BlockLogin = viewModel.BlockLogin,
                        Email = viewModel.Email,
                        LastNames = viewModel.LastNames,
                        LogonName = viewModel.Email,
                        Names = viewModel.Names,
                        Password = viewModel.Password
                    };

                    var response = await identityClient.CreateUserAsync(requestModel);

                    if (response.IsValid)
                        return RedirectToAction(nameof(Index));
                    else
                        this.AddMessages(response);
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> Edit(UserEditViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = await identityClient.GetAsync(viewModel.UserId);

                    UserUpdateRequest requestModel = new UserUpdateRequest()
                    {
                        BlockLogin = model.BlockLogin,
                        Email = model.Email,
                        LastNames = model.LastNames,
                        LogonName = model.Email,
                        Names = model.Names,
                        UserId = model.UserId
                    };

                    requestModel.BlockLogin = viewModel.BlockLogin;
                    requestModel.LastNames = viewModel.LastNames;
                    requestModel.Names = viewModel.Names;

                    await identityClient.UpdateUserAsync(requestModel);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return View(viewModel);
        }


        [HttpGet]
        [Authorize(Roles = SecurityRoles.UserEditor)]
        public async Task<IActionResult> Search(string search, int page, int pageSize)
        {
            var response = await identityClient.GetAsync(
                QueryRequest.Builder().
                SetPage(page).
                SetPageSize(pageSize).
                AddCondition("LogonNameFullName", search, RelationalOperators.Contain).
                AddFieldSort("FullName"));

            return Json(new
            {
                results = response.Data.Select(p => new { text = p.LogonNameFullName, id = p.UserId }),
                pageSize = pageSize,
                totalCount = response.TotalCount
            });
        }

        #region methods

        private void InitIdentityClient()
        {
            var profile = apiProfileManager.Get("Identity.User");

            identityClient = new IdentityClient(
                profile.BaseAddress,
                profile.ClientId,
                profile.ClientSecret,
                GlobalOptions.DefaultTenantId);
        }

        private async Task PrepareEditUserGroupModel(UserEditGroupViewModel model)
        {
            var userGroups = await identityClient.GetUserGroupsAsync(QueryRequest.Builder());
            model.UserGroupSelectList = new SelectList(userGroups.Data, "UserGroupId", "Name");
        }

        #endregion


    }
}