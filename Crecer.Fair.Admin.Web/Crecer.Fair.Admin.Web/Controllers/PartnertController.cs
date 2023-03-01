using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Crecer.Fair.Admin.Web.Models;
using Crecer.Fair.Admin.Web.Services;
using Crecer.Fair.Admin.Web.Services.Models;
using Ecubytes.AspNetCore.Mvc.DataTables;
using Ecubytes.Extensions.AspNetCore.Mvc.DataTable.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;

namespace Crecer.Fair.Admin.Web.Controllers
{
    public class PartnertController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly IPartnertService partnertService;
        public PartnertController(
            IPartnertService partnertService)
        {
            this.partnertService = partnertService;
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.PartnertEditor)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = SecurityRoles.PartnertEditor)]
        public async Task<IActionResult> GetList([FromQuery] IDataTablesRequest request, bool export = false, int? totalCount = null)
        {
            var queryRequest = request.ToQueryRequest();

            var response = await this.partnertService.GetAsync(queryRequest);

            if (!export)
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
                    workSheet.Cells[1, 1].Value = "Identificación";
                    workSheet.Cells[1, 2].Value = "Nombres";
                    workSheet.Cells[1, 3].Value = "Apellidos";
                    workSheet.Cells[1, 4].Value = "Email";
                    workSheet.Cells[1, 5].Value = "Teléfono";                    
                    workSheet.Cells[1, 6].Value = "Usuario Activo";
                    workSheet.Row(1).Height = 20;
                    workSheet.Row(1).Style.Font.Bold = true;

                    int row = 2;
                    foreach (var item in response.Data)
                    {
                        workSheet.Cells[row, 1].Value = item.Identification;
                        workSheet.Cells[row, 2].Value = item.Names;
                        workSheet.Cells[row, 3].Value = item.LastNames;
                        workSheet.Cells[row, 4].Value = item.Email;
                        workSheet.Cells[row, 5].Value = item.PhoneNumber;                        
                        workSheet.Cells[row, 6].Value = item.HasUser ? "SI" : "NO";
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
                string excelName = $"Socios-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }

        [HttpPost]
        [Authorize(Roles = SecurityRoles.PartnertEditor)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormFile batchPartnert)
        {
            if (ModelState.IsValid)
            {
                if (batchPartnert?.Length > 0)
                {
                    using (var stream = batchPartnert.OpenReadStream())
                    {
                        List<PartnertImportRequestDTO> partnerts = new List<PartnertImportRequestDTO>();
                        try
                        {
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets.First();//package.Workbook.Worksheets[0];
                                var rowCount = worksheet.Dimension.Rows;

                                for (var row = 2; row <= rowCount; row++)
                                {
                                    try
                                    {

                                        var identification = worksheet.Cells[row, 1].Value?.ToString();
                                        var names = worksheet.Cells[row, 2].Value?.ToString();
                                        var lastNames = worksheet.Cells[row, 3].Value?.ToString();

                                        var partnert = new PartnertImportRequestDTO()
                                        {
                                            Identification = identification,
                                            LastNames = lastNames,
                                            Names = names
                                        };

                                        partnerts.Add(partnert);
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.LogError(ex, $"Error al leer row {row}");
                                    }
                                }

                                var result = await partnertService.ImportAsync(partnerts);
                                this.AddMessages(result);

                                if (this.IsValidState)
                                    this.AddDefaultSuccessMessage();
                            }

                            return RedirectToAction(nameof(Index));

                        }
                        catch (Exception ex)
                        {
                            this.AddDefaultErrorMessage(ex);
                        }
                    }
                }
            }

            return View();
        }
    }
}
