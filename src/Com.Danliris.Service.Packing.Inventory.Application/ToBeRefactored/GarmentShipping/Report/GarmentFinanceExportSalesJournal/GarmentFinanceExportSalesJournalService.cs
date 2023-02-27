using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentShippingInvoice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.Repositories.GarmentShipping.GarmentPackingList;
using Com.Danliris.Service.Packing.Inventory.Infrastructure.IdentityProvider;
using System.Data;
using Com.Danliris.Service.Packing.Inventory.Application.Utilities;
using System.Threading.Tasks;
using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Newtonsoft.Json;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.Utilities;
using OfficeOpenXml;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.Report
{
    public class GarmentFinanceExportSalesJournalService : IGarmentFinanceExportSalesJournalService
    {
        private readonly IGarmentShippingInvoiceRepository repository;
        private readonly IGarmentPackingListRepository plrepository;
        private readonly IGarmentShippingInvoiceItemRepository itemrepository;

        private readonly IIdentityProvider _identityProvider;
        private readonly IServiceProvider _serviceProvider;

        public GarmentFinanceExportSalesJournalService(IServiceProvider serviceProvider)
        {
            repository = serviceProvider.GetService<IGarmentShippingInvoiceRepository>();
            plrepository = serviceProvider.GetService<IGarmentPackingListRepository>();
            itemrepository = serviceProvider.GetService<IGarmentShippingInvoiceItemRepository>();
            _identityProvider = serviceProvider.GetService<IIdentityProvider>();
            _serviceProvider = serviceProvider;
        }
        private GarmentCurrency GetCurrencyPEBDate(string stringDate)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var httpClient = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));

            var currencyUri = ApplicationSetting.CoreEndpoint + $"master/garment-detail-currencies/sales-debtor-currencies-peb?stringDate={stringDate}";
            var currencyResponse = httpClient.GetAsync(currencyUri).Result.Content.ReadAsStringAsync();

            var currencyResult = new BaseResponse<GarmentCurrency>()
            {
                data = new GarmentCurrency()
            };
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(currencyResponse.Result);
            var json = result.Single(p => p.Key.Equals("data")).Value;
            var data = JsonConvert.DeserializeObject<GarmentCurrency>(json.ToString());

            return data;
        }
        public List<GarmentFinanceExportSalesJournalViewModel> GetReportQuery(DateTime? dateFrom, DateTime? dateTo, int offset)
        {

            //DateTime dateFrom = new DateTime(year, month, 1);
            //int nextYear = month == 12 ? year + 1 : year;
            //int nextMonth = month == 12 ? 1 : month + 1;
            //DateTime dateTo = new DateTime(nextYear, nextMonth, 1);

            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            List<GarmentFinanceExportSalesJournalViewModel> data = new List<GarmentFinanceExportSalesJournalViewModel>();
            List<GarmentFinanceExportSalesJournalTempViewModel> data1 = new List<GarmentFinanceExportSalesJournalTempViewModel>();
            List<GarmentFinanceExportSalesJournalTempViewModel> datax = new List<GarmentFinanceExportSalesJournalTempViewModel>();

            var queryInv = repository.ReadAll();
            var queryInvItm = itemrepository.ReadAll();

            var queryPL = plrepository.ReadAll()
                .Where(w => w.TruckingDate.AddHours(offset).Date >= DateFrom && w.TruckingDate.AddHours(offset).Date <= DateTo.Date
                    && w.PackingListType != "LOKAL" && (w.InvoiceType == "AG" || w.InvoiceType == "DS" || w.InvoiceType == "AGR" || w.InvoiceType == "SMR"));

            var joinQuery = from a in queryInv
                            join c in queryInvItm on a.Id equals c.GarmentShippingInvoiceId
                            join b in queryPL on a.PackingListId equals b.Id
                            where a.IsDeleted == false && b.IsDeleted == false
                            select new GarmentFinanceExportSalesJournalTempViewModel
                            {
                                InvoiceType = b.InvoiceType,
                                RO_Number = c.RONo,
                                TotalAmount = a.TotalAmount,
                                PEBDate = a.PEBDate,
                                Qty = c.Quantity,
                                Price = c.Price,
                                Rate = 0,
                                AmountCC = 0,
                            };

            //List<dataQuery> dataQuery = new List<dataQuery>();

            //foreach (var invoice in joinQuery)
            //{
            //    GarmentCurrency currency = GetCurrencyPEBDate(invoice.PEBDate.Date.ToShortDateString());
            //    var rate = currency != null ? Convert.ToDecimal(currency.rate) : 0;
            //    invoice.Rate = rate;
            //    joinQuery.Add(invoice);
            //}

            //
            foreach (GarmentFinanceExportSalesJournalTempViewModel i in joinQuery)
            {
                GarmentCurrency currency = GetCurrencyPEBDate(i.PEBDate.Date.ToShortDateString());
                var rate = currency != null ? Convert.ToDecimal(currency.rate) : 0;

                data1.Add(new GarmentFinanceExportSalesJournalTempViewModel
                {
                    InvoiceType = i.InvoiceType,
                    RO_Number = i.RO_Number,
                    PEBDate = i.PEBDate,
                    TotalAmount = i.TotalAmount,
                    Rate = rate,
                    Qty = i.Qty,
                    Price = i.Price,
                    AmountCC = 0,
                });
            };
            //

            var join = from a in data1
                       select new GarmentFinanceExportSalesJournalViewModel
                       {
                           remark = a.InvoiceType == "AG" || a.InvoiceType == "DS" ? "       PENJUALAN EXPORT (AG2)" : "       PENJUALAN LAIN-LAIN EXPORT (AG2)",
                           credit = a.TotalAmount * a.Rate,
                           debit = 0,
                           account = a.InvoiceType == "AG" || a.InvoiceType == "DS" ? "420.00.2.000" : "421.00.2.000"
                       };

            var debit = new GarmentFinanceExportSalesJournalViewModel
            {
                remark = "PIUTANG USAHA EXPORT (AG2)",
                credit = 0,
                debit = join.Sum(a => a.credit),
                account = "112.00.3.000"
            };
            if (debit.debit > 0)
            {
                data.Add(debit);
            }
            else
            {
                var debitx = new GarmentFinanceExportSalesJournalViewModel
                {
                    remark = "PIUTANG USAHA EXPORT (AG2)",
                    credit = 0,
                    debit = 0,
                    account = "112.00.3.000"
                };
                data.Add(debitx);
            }

            var sumquery = join.ToList()
                   .GroupBy(x => new { x.remark, x.account }, (key, group) => new
                   {
                       Remark = key.remark,
                       Account = key.account,
                       Credit = group.Sum(s => s.credit)
                   }).OrderBy(a => a.Remark);
            foreach (var item in sumquery)
            {
                var obj = new GarmentFinanceExportSalesJournalViewModel
                {
                    remark = item.Remark,
                    credit = item.Credit,
                    debit = 0,
                    account = item.Account
                };

                data.Add(obj);
            }
            //
            if (join.ToList().Count == 0)
            {
                var credit1x = new GarmentFinanceExportSalesJournalViewModel
                {
                    remark = "       PENJUALAN EXPORT (AG2)",
                    credit = 0,
                    debit = 0,
                    account = "420.00.2.000"
                };
                data.Add(credit1x);

                //var credit2x = new GarmentFinanceExportSalesJournalViewModel
                //{
                //    remark = "       PENJUALAN LAIN-LAIN EXPORT (AG2)",
                //    credit = 0,
                //    debit = 0,
                //    account = "421.00.2.000"
                //};
                //data.Add(credit2x);
            }

            //
            foreach (GarmentFinanceExportSalesJournalTempViewModel i in data1)
            {
                var data2 = GetCostCalculation(i.RO_Number);

                datax.Add(new GarmentFinanceExportSalesJournalTempViewModel
                {
                    InvoiceType = i.InvoiceType,
                    RO_Number = i.RO_Number,
                    PEBDate = i.PEBDate,
                    TotalAmount = i.TotalAmount,
                    Rate = i.Rate,
                    Qty = i.Qty,
                    Price = i.Price,
                    AmountCC = data2 == null || data2.Count == 0 ? 0 : data2.FirstOrDefault().AmountCC * i.Qty,
                });
            };
            //
            var debit3 = new GarmentFinanceExportSalesJournalViewModel
            {
                remark = "HARGA POKOK PENJUALAN(AG2)",
                credit = 0,
                debit = Convert.ToDecimal(datax.Sum(a => a.AmountCC)),
                account = "500.00.2.000",
            };
            if (debit3.debit > 0)
            {
                data.Add(debit3);
            }
            else
            {
                var debit3x = new GarmentFinanceExportSalesJournalViewModel
                {
                    remark = "HARGA POKOK PENJUALAN(AG2)",
                    credit = 0,
                    debit = 0,
                    account = "500.00.2.000",
                };
                data.Add(debit3x);
            }
            //
            var stock = new GarmentFinanceExportSalesJournalViewModel
            {
                remark = "       PERSEDIAAN BARANG JADI (AG2)",
                credit = Convert.ToDecimal(datax.Sum(a => a.AmountCC)),
                debit = 0,
                account = "114.01.2.000",
            };

            if (stock.credit > 0)
            {
                data.Add(stock);
            }
            else
            {
                var stockx = new GarmentFinanceExportSalesJournalViewModel
                {
                    remark = "       PERSEDIAAN BARANG JADI (AG2)",
                    credit = 0,
                    debit = 0,
                    account = "114.01.2.000",
                };
                data.Add(stockx);
            }

            var total = new GarmentFinanceExportSalesJournalViewModel
            {
                remark = "",
                credit = join.Sum(a => a.credit) + Convert.ToDecimal(datax.Sum(a => a.AmountCC)),
                debit = join.Sum(a => a.credit) + Convert.ToDecimal(datax.Sum(a => a.AmountCC)),
                account = "JUMLAH"
            };
            if (total.credit > 0)
            {
                data.Add(total);
            }
            else
            {
                var totalx = new GarmentFinanceExportSalesJournalViewModel
                {
                    remark = "",
                    credit = 0,
                    debit = 0,
                    account = "JUMLAH"
                };
                data.Add(totalx);
            }
            return data;
        }

        public List<CostCalculationGarmentForJournal> GetCostCalculation(string RO_Number)
        {
            string costcalcUri = "cost-calculation-garments/dataforjournal";
            IHttpClientService httpClient = (IHttpClientService)_serviceProvider.GetService(typeof(IHttpClientService));

            var response = httpClient.GetAsync($"{ApplicationSetting.SalesEndpoint}{costcalcUri}?RO_Number={RO_Number}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                List<CostCalculationGarmentForJournal> viewModel;
                if (result.GetValueOrDefault("data") == null)
                {
                    viewModel = null;
                }
                else
                {
                    viewModel = JsonConvert.DeserializeObject<List<CostCalculationGarmentForJournal>>(result.GetValueOrDefault("data").ToString());
                }
                return viewModel;
            }
            else
            {
                return null;
            }
        }

        public List<GarmentFinanceExportSalesJournalViewModel> GetReportData(DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            var Query = GetReportQuery(dateFrom, dateTo, offset);
            return Query.ToList();
        }

        public MemoryStream GenerateExcel(DateTime? dateFrom, DateTime? dateTo, int offset)
        {
            DateTime DateFrom = dateFrom == null ? new DateTime(1970, 1, 1) : (DateTime)dateFrom;
            DateTime DateTo = dateTo == null ? DateTime.Now : (DateTime)dateTo;

            var Query = GetReportQuery(dateFrom, dateTo, offset);
            DataTable result = new DataTable();

            result.Columns.Add(new DataColumn() { ColumnName = "AKUN DAN KETERANGAN", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "AKUN", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "DEBET", DataType = typeof(string) });
            result.Columns.Add(new DataColumn() { ColumnName = "KREDIT", DataType = typeof(string) });

            ExcelPackage package = new ExcelPackage();

            if (Query.ToArray().Count() == 0)
            {
                result.Rows.Add("", "", 0, 0);
                bool styling = true;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);

                    sheet.Column(1).Width = 50;
                    sheet.Column(2).Width = 15;
                    sheet.Column(3).Width = 20;
                    sheet.Column(4).Width = 20;

                    #region KopTable
                    sheet.Cells[$"A1:D1"].Value = "PT AMBASSADOR GARMINDO";
                    sheet.Cells[$"A1:D1"].Merge = true;
                    sheet.Cells[$"A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A1:D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A1:D1"].Style.Font.Bold = true;

                    sheet.Cells[$"A2:D2"].Value = "ACCOUNTING DEPT.";
                    sheet.Cells[$"A2:D2"].Merge = true;
                    sheet.Cells[$"A2:D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A2:D2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A2:D2"].Style.Font.Bold = true;

                    sheet.Cells[$"A4:D4"].Value = "IKHTISAR JURNAL";
                    sheet.Cells[$"A4:D4"].Merge = true;
                    sheet.Cells[$"A4:D4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.Font.Bold = true;

                    sheet.Cells[$"C5"].Value = "BUKU HARIAN";
                    sheet.Cells[$"C5"].Style.Font.Bold = true;
                    sheet.Cells[$"D5"].Value = ": PENJUALAN EXPORT";
                    sheet.Cells[$"D5"].Style.Font.Bold = true;

                    sheet.Cells[$"C6"].Value = "PERIODE";
                    sheet.Cells[$"C6"].Style.Font.Bold = true;
                    sheet.Cells[$"D6"].Value = ": " + DateFrom.ToString("dd-MM-yyyy") + " S/D " + DateTo.ToString("dd-MM-yyyy");
                    sheet.Cells[$"D6"].Style.Font.Bold = true;

                    #endregion
                    sheet.Cells["A8"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }
            else
            {
                int index = 0;
                foreach (var d in Query)
                {
                    index++;

                    result.Rows.Add(d.remark, d.account, d.debit, d.credit);
                }

                bool styling = true;

                foreach (KeyValuePair<DataTable, String> item in new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Territory") })
                {
                    var sheet = package.Workbook.Worksheets.Add(item.Value);

                    #region KopTable
                    sheet.Cells[$"A1:D1"].Value = "PT AMBASSADOR GARMINDO";
                    sheet.Cells[$"A1:D1"].Merge = true;
                    sheet.Cells[$"A1:D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A1:D1"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A1:D1"].Style.Font.Bold = true;

                    sheet.Cells[$"A2:D2"].Value = "ACCOUNTING DEPT.";
                    sheet.Cells[$"A2:D2"].Merge = true;
                    sheet.Cells[$"A2:D2"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    sheet.Cells[$"A2:D2"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A2:D2"].Style.Font.Bold = true;

                    sheet.Cells[$"A4:D4"].Value = "IKHTISAR JURNAL";
                    sheet.Cells[$"A4:D4"].Merge = true;
                    sheet.Cells[$"A4:D4"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    sheet.Cells[$"A4:D4"].Style.Font.Bold = true;

                    sheet.Cells[$"C5"].Value = "BUKU HARIAN";
                    sheet.Cells[$"C5"].Style.Font.Bold = true;
                    sheet.Cells[$"D5"].Value = ": PENJUALAN EXPORT";
                    sheet.Cells[$"D5"].Style.Font.Bold = true;

                    sheet.Cells[$"C6"].Value = "PERIODE";
                    sheet.Cells[$"C6"].Style.Font.Bold = true;
                    sheet.Cells[$"D6"].Value = ": " + DateFrom.ToString("dd-MM-yyyy") + " S/D " + DateTo.ToString("dd-MM-yyyy");
                    sheet.Cells[$"D6"].Style.Font.Bold = true;

                    #endregion
                    sheet.Cells["A8"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

                    //sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                }
            }

            var stream = new MemoryStream();
            package.SaveAs(stream);

            return stream;
        }
        //

        //public MemoryStream GenerateExcel(DateTime? dateFrom, DateTime? dateTo, int offset)
        //{
        //    var Query = GetReportQuery(dateFrom, dateTo, offset);
        //    DataTable result = new DataTable();

        //    result.Columns.Add(new DataColumn() { ColumnName = "AKUN DAN KETERANGAN", DataType = typeof(string) });
        //    result.Columns.Add(new DataColumn() { ColumnName = "AKUN", DataType = typeof(string) });
        //    result.Columns.Add(new DataColumn() { ColumnName = "DEBET", DataType = typeof(string) });
        //    result.Columns.Add(new DataColumn() { ColumnName = "KREDIT", DataType = typeof(string) });

        //    if (Query.ToArray().Count() == 0)
        //        result.Rows.Add("", "", "", "");
        //    else
        //    {
        //        foreach (var d in Query)
        //        {
        //            string Credit = d.credit > 0 ? string.Format("{0:N2}", d.credit) : "";
        //            string Debit = d.debit > 0 ? string.Format("{0:N2}", d.debit) : "";

        //            result.Rows.Add(d.remark, d.account, Debit, Credit);
        //        }
        //    }

        //    return Excel.CreateExcel(new List<KeyValuePair<DataTable, string>>() { new KeyValuePair<DataTable, string>(result, "Sheet1") }, true);
        //}
    }

    public class BaseResponse<T>
    {
        public string apiVersion { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public T data { get; set; }

        public static implicit operator BaseResponse<T>(BaseResponse<string> v)
        {
            throw new NotImplementedException();
        }
    }

    public class dataQuery
    {
        public string InvoiceType { get; set; }
        public DateTimeOffset PEBDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Rate { get; set; }
    }
}
