using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Bike.Data;
using Bike.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Linq;
using Route = Bike.Models.Route;

namespace Bike.Controllers
{
    public class OtchetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OtchetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OtchetController
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(IFormFile fileExcel)
        {
            using (XLWorkbook workbook = new XLWorkbook(fileExcel.OpenReadStream()))
            {
                List<Address_ImpExp> Address_ImpExps = new List<Address_ImpExp>();
                List<Bike_ImpExp> Bike_ImpExps = new List<Bike_ImpExp>();

                foreach (IXLWorksheet worksheet in workbook.Worksheets)
                {
                    if (worksheet.Name == "Address")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Address address = new Address();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            address.MainAddressId = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "MainAddressId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            address.Street = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Street").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            address.House = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "House").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            _context.Address.Add(address);

                            _context.SaveChanges();

                            Address_ImpExps.Add(new Address_ImpExp { AddressSubd = address.AddressId, AddressExcel = int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "AddressId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString()) }); ;
                        }
                    }

                    if (worksheet.Name == "BikeType")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            BikeType bikeType = new BikeType();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            bikeType.Name = row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Name").RangeAddress.FirstAddress.ColumnNumber).Value.ToString();
                            bikeType.Complexity = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Complexity").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            bikeType.Speed = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Speed").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());
                            bikeType.Time = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Time").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());

                            _context.BikeType.Add(bikeType);

                            _context.SaveChanges();

                            Bike_ImpExps.Add(new Bike_ImpExp { BikeSubd = bikeType.BikeTypeId, BikeExcel = int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "BikeTypeId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString()) }); ;
                        }
                    }

                    if (worksheet.Name == "Route")
                    {
                        foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                        {
                            Route routes = new Route();
                            var range = worksheet.RangeUsed();

                            var table = range.AsTable();

                            routes.AddressId1 = Address_ImpExps.FirstOrDefault(c => c.AddressExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "AddressId1").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).AddressSubd;
                            routes.AddressId2 = Address_ImpExps.FirstOrDefault(c => c.AddressExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "AddressId2").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).AddressSubd;
                            routes.BikeTypeId = Bike_ImpExps.FirstOrDefault(c => c.BikeExcel == int.Parse(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "BikeTypeId").RangeAddress.FirstAddress.ColumnNumber).Value.ToString())).BikeSubd;
                            routes.Time = Convert.ToInt32(row.Cell(table.FindColumn(c => c.FirstCell().Value.ToString() == "Time").RangeAddress.FirstAddress.ColumnNumber).Value.ToString());

                            _context.Routes.Add(routes);

                            _context.SaveChanges();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: OtchetController/Details/5
        public ActionResult Export(int? id)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Otchet");
                worksheet.Cell(1, 1).Value = "Тип велосипеда";
                worksheet.Cell(1, 2).Value = "Количество маршрутов";

                worksheet.Row(1).Style.Font.Bold = true;

                var otch = _context.Set<Route_CountOtchet>().FromSqlInterpolated($"EXEC Otchet").ToList();
                int i = 2;
                foreach (Route_CountOtchet item in otch)
                {
                    worksheet.Cell(i, 1).Value = item.nm;
                    worksheet.Cell(i, 2).Value = item.kol;
                    i++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet")
                    {
                        FileDownloadName = $"Otchet_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }

        public ActionResult Export2()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet1 = workbook.Worksheets.Add("Address");
                worksheet1.Cell(1, 1).Value = "AddressId";
                worksheet1.Cell(1, 2).Value = "MainAddressId";
                worksheet1.Cell(1, 3).Value = "Street";
                worksheet1.Cell(1, 4).Value = "House";

                int i1 = 2;

                worksheet1.Row(1).Style.Font.Bold = true;

                foreach (Address item in _context.Address)
                {
                    worksheet1.Cell(i1, 1).Value = item.AddressId;
                    worksheet1.Cell(i1, 2).Value = item.MainAddressId;
                    worksheet1.Cell(i1, 3).Value = item.Street;
                    worksheet1.Cell(i1, 4).Value = item.House;

                    i1++;
                }

                var worksheet2 = workbook.Worksheets.Add("BikeType");
                worksheet2.Cell(1, 1).Value = "BikeTypeId";
                worksheet2.Cell(1, 2).Value = "Name";
                worksheet2.Cell(1, 3).Value = "Complexity";
                worksheet2.Cell(1, 4).Value = "Speed";
                worksheet2.Cell(1, 5).Value = "Time";

                int i2 = 2;

                worksheet2.Row(1).Style.Font.Bold = true;

                foreach (BikeType item in _context.BikeType)
                {
                    worksheet2.Cell(i2, 1).Value = item.BikeTypeId;
                    worksheet2.Cell(i2, 2).Value = item.Name;
                    worksheet2.Cell(i2, 3).Value = item.Complexity;
                    worksheet2.Cell(i2, 4).Value = item.Speed;
                    worksheet2.Cell(i2, 5).Value = item.Time;

                    i2++;
                }

                var worksheet = workbook.Worksheets.Add("Route");
                worksheet.Cell(1, 1).Value = "RouteId";
                worksheet.Cell(1, 2).Value = "AddressId1";
                worksheet.Cell(1, 3).Value = "AddressId2";
                worksheet.Cell(1, 4).Value = "BikeTypeId";
                worksheet.Cell(1, 5).Value = "Time";
                worksheet.Cell(1, 6).Value = "TimeResult";

                worksheet.Row(1).Style.Font.Bold = true;

                int i = 2;
                foreach (Route item in _context.Routes)
                {
                    worksheet.Cell(i, 1).Value = item.RouteId;
                    worksheet.Cell(i, 2).Value = item.AddressId1;
                    worksheet.Cell(i, 3).Value = item.AddressId2;
                    worksheet.Cell(i, 4).Value = item.BikeTypeId;
                    worksheet.Cell(i, 5).Value = item.Time;
                    worksheet.Cell(i, 6).Value = item.TimeResult;

                    i++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreedsheetml.sheet")
                    {
                        FileDownloadName = $"Export_{DateTime.UtcNow.ToLongDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
