using Com.Danliris.Service.Packing.Inventory.Application.CommonViewModelObjectProperties;
using Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentPackingList;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Com.Danliris.Service.Packing.Inventory.Application.ToBeRefactored.GarmentShipping.GarmentShippingInvoice
{
    public class GarmentShippingInvoiceLocalPdfTemplate
    {
        public MemoryStream GeneratePdfTemplate(GarmentShippingInvoiceViewModel viewModel, Buyer buyer, BankAccount bank, GarmentPackingListViewModel pl, int timeoffset)
        {
            const int MARGIN = 20;

            Font header_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 14);
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font body_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
            Font normal_font_underlined = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, Font.UNDERLINE);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            //Font body_bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);

            //Document document = new Document(PageSize.A4, MARGIN, MARGIN, 290, 150);
            Document document = new Document(PageSize.A4, MARGIN, MARGIN, 200, 150);
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            writer.PageEvent = new GarmentShippingInvoiceLocalPDFTemplatePageEvent(viewModel, timeoffset);

            document.Open();
            #region LC
            PdfPTable tableLC = new PdfPTable(3);
            tableLC.SetWidths(new float[] { 2f, 0.1f, 6f });

            if (pl.PaymentTerm == "LC")
            {
                PdfPCell cellLCContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellLCContentLeft.AddElement(new Phrase("LETTER OF CREDIT NUMBER ", normal_font));
                cellLCContentLeft.AddElement(new Phrase("LC DATE ", normal_font));
                cellLCContentLeft.AddElement(new Phrase("ISSUED BY ", normal_font));
                tableLC.AddCell(cellLCContentLeft);

                PdfPCell cellLCContentCenter = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellLCContentCenter.AddElement(new Phrase(": ", normal_font));
                cellLCContentCenter.AddElement(new Phrase(": ", normal_font));
                cellLCContentCenter.AddElement(new Phrase(": ", normal_font));
                tableLC.AddCell(cellLCContentCenter);

                PdfPCell cellLCContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellLCContentRight.AddElement(new Phrase(viewModel.LCNo, normal_font));
                cellLCContentRight.AddElement(new Phrase(pl.LCDate.GetValueOrDefault().ToOffset(new TimeSpan(timeoffset, 0, 0)).ToString("dd MMMM yyyy", new System.Globalization.CultureInfo("en-EN")), normal_font));
                cellLCContentRight.AddElement(new Phrase(viewModel.IssuedBy, normal_font));
                tableLC.AddCell(cellLCContentRight);
            }
            else
            {
                PdfPCell cellLCContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellLCContentLeft.AddElement(new Phrase("PAYMENT TERM ", normal_font));
                tableLC.AddCell(cellLCContentLeft);

                PdfPCell cellLCContentCenter = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellLCContentCenter.AddElement(new Phrase(": ", normal_font));
                tableLC.AddCell(cellLCContentCenter);

                PdfPCell cellLCContentRight = new PdfPCell() { Border = Rectangle.NO_BORDER };
                cellLCContentRight.AddElement(new Phrase("TT PAYMENT", normal_font));
                tableLC.AddCell(cellLCContentRight);
            }

            PdfPCell cellLC = new PdfPCell(tableLC);
            tableLC.ExtendLastRow = false;
            tableLC.SpacingAfter = 4f;
            document.Add(tableLC);
            #endregion

            #region Body Table
            //PdfPCell bodyTableHeader = new PdfPCell() { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE };



            PdfPTable bodyTable = new PdfPTable(8);
            //float[] bodyTableWidths = new float[] { 1.8f, 1.8f, 1.8f, 1.8f, 0.6f, 0.7f, 1f, 1.3f };
            //float[] bodyTableWidths = new float[] { 1.9f, 1.9f, 1.8f, 1.8f, 0.5f, 0.6f, 1f, 1.1f };
            //bodyTable.SetWidths(bodyTableWidths);
            //float[] bodyTableWidths = new float[] { 1.9f, 1.9f, 1.8f, 1.8f, 0.5f, 0.6f, 1f, 1.1f };
            bodyTable.SetWidths(new float[] { 1.9f, 1.9f, 1.8f, 1.8f, 0.5f, 0.6f, 1f, 1.1f });
            bodyTable.WidthPercentage = 100;


            #region Set Body Table Header
            PdfPCell bodyTableHeader = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            //PdfPCell table1RightCellHeader = new PdfPCell() { FixedHeight = 20, Colspan = 4 };
            PdfPCell bodyTableHeader_Line = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER, Colspan = 8, Padding = 0.5f, Phrase = new Phrase("") };
            bodyTable.AddCell(bodyTableHeader_Line);

            bodyTableHeader.Phrase = new Phrase("DESCRIPTION", normal_font);
            bodyTableHeader.Rowspan = 1;
            bodyTableHeader.Colspan = 4;
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("QUANTITY", normal_font);
            bodyTableHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
            bodyTableHeader.Colspan = 2;
            bodyTableHeader.Rowspan = 2;

            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("UNIT PRICE\n" + viewModel.CPrice + "(RP)", normal_font);
            bodyTableHeader.Colspan = 1;
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("TOTAL PRICE\n" + viewModel.CPrice + "(RP)", normal_font);
            bodyTableHeader.Colspan = 1;
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("PO Buyer", normal_font);
            bodyTableHeader.Rowspan = 1;
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("Article", normal_font);
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("Colour", normal_font);
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("Remark", normal_font);
            bodyTable.AddCell(bodyTableHeader);

            bodyTableHeader.Phrase = new Phrase("", normal_font);
            bodyTableHeader.Border = Rectangle.NO_BORDER;
            bodyTableHeader.Colspan = 2;
            bodyTable.AddCell(bodyTableHeader);
            
            #endregion

            #region Set Body Table Value
            PdfPCell bodyTableCellRightBorder = new PdfPCell() { MinimumHeight = 15, Border = Rectangle.RIGHT_BORDER };
            PdfPCell bodyTableCellLeftBorder = new PdfPCell() { MinimumHeight = 15, Border = Rectangle.LEFT_BORDER };
            PdfPCell bodyTableCellCenterBorder = new PdfPCell() { MinimumHeight = 15, Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER };

            bodyTableCellLeftBorder.Phrase = new Phrase($"{viewModel.Description}", body_font);
            bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.SetLeading(0, 1.3f);
            bodyTableCellLeftBorder.Colspan = 4;
            bodyTable.AddCell(bodyTableCellLeftBorder);

            bodyTableCellLeftBorder.Phrase = new Phrase("", body_font);
            bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.Colspan = 2;
            bodyTable.AddCell(bodyTableCellLeftBorder);

            bodyTableCellCenterBorder.Phrase = new Phrase("", body_font);
            bodyTableCellCenterBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellCenterBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellCenterBorder.SetLeading(0, 1.3f);
            bodyTable.AddCell(bodyTableCellCenterBorder);

            bodyTableCellRightBorder.Phrase = new Phrase("", body_font);
            bodyTableCellRightBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellRightBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellRightBorder.SetLeading(0, 1.3f);
            bodyTable.AddCell(bodyTableCellRightBorder);

            
            bodyTableCellLeftBorder.Phrase = new Phrase($"{viewModel.Remark}", body_font);
            bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.Colspan = 4;
            bodyTable.AddCell(bodyTableCellLeftBorder);

            bodyTableCellLeftBorder.Phrase = new Phrase("", body_font);
            bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.Colspan = 2;
            bodyTable.AddCell(bodyTableCellLeftBorder);

            bodyTableCellCenterBorder.Phrase = new Phrase("", body_font);
            bodyTableCellCenterBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellCenterBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTable.AddCell(bodyTableCellCenterBorder);

            bodyTableCellRightBorder.Phrase = new Phrase("", body_font);
            bodyTableCellRightBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellRightBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTable.AddCell(bodyTableCellRightBorder);

            //SPACE
            bodyTableCellLeftBorder.Phrase = new Phrase("", body_font);
            bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.Colspan = 4;
            bodyTable.AddCell(bodyTableCellLeftBorder);

            bodyTableCellLeftBorder.Phrase = new Phrase("", body_font);
            bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellLeftBorder.Colspan = 2;
            bodyTable.AddCell(bodyTableCellLeftBorder);

            bodyTableCellCenterBorder.Phrase = new Phrase("", body_font);
            bodyTableCellCenterBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellCenterBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTable.AddCell(bodyTableCellCenterBorder);

            bodyTableCellRightBorder.Phrase = new Phrase("", body_font);
            bodyTableCellRightBorder.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellRightBorder.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTable.AddCell(bodyTableCellRightBorder);


            decimal totalAmount = 0;
            double totalQuantity = 0;

            Dictionary<string, double> total = new Dictionary<string, double>();

            foreach (var item in viewModel.Items.OrderBy(o => o.ComodityDesc))
            {
                totalAmount += item.Amount;
                totalQuantity += item.Quantity;

                bodyTableCellLeftBorder.Phrase = new Phrase($"{item.ComodityDesc}", body_font);
                bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTableCellLeftBorder.Colspan = 1;
                bodyTableCellLeftBorder.Border = Rectangle.LEFT_BORDER;
                bodyTable.AddCell(bodyTableCellLeftBorder);

                bodyTableCellLeftBorder.Phrase = new Phrase($"{item.Desc2}", body_font);
                bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTableCellLeftBorder.Border = Rectangle.NO_BORDER;
                bodyTableCellLeftBorder.Colspan = 1;
                bodyTable.AddCell(bodyTableCellLeftBorder);

                bodyTableCellLeftBorder.Phrase = new Phrase($"{item.Desc3}", body_font);
                bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTableCellLeftBorder.Border = Rectangle.NO_BORDER;
                bodyTableCellLeftBorder.Colspan = 1;
                bodyTable.AddCell(bodyTableCellLeftBorder);

                bodyTableCellLeftBorder.Phrase = new Phrase($"{item.Desc4}", body_font);
                bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTableCellLeftBorder.Border = Rectangle.RIGHT_BORDER;
                bodyTableCellLeftBorder.Colspan = 1;
                bodyTable.AddCell(bodyTableCellLeftBorder);

                bodyTableCellLeftBorder.Phrase = new Phrase(string.Format("{0:n0}", item.Quantity), body_font);
                bodyTableCellLeftBorder.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyTableCellLeftBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTableCellLeftBorder.BorderColorRight = BaseColor.White;
                bodyTableCellLeftBorder.Border = Rectangle.LEFT_BORDER;
                bodyTable.AddCell(bodyTableCellLeftBorder);

                bodyTableCellRightBorder.Phrase = new Phrase(item.Uom.Unit, body_font);
                bodyTableCellRightBorder.HorizontalAlignment = Element.ALIGN_LEFT;
                bodyTableCellRightBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTableCellRightBorder.BorderColorLeft = BaseColor.White;
                bodyTable.AddCell(bodyTableCellRightBorder);

                bodyTableCellRightBorder.Phrase = new Phrase(item.Price != 0 ? string.Format("{0:n4}", item.Price) : "", body_font);
                bodyTableCellRightBorder.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyTableCellRightBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTable.AddCell(bodyTableCellRightBorder);

                bodyTableCellRightBorder.Phrase = new Phrase(item.Amount != 0 ? string.Format("{0:n2}", item.Amount) : "", body_font);
                bodyTableCellRightBorder.HorizontalAlignment = Element.ALIGN_RIGHT;
                bodyTableCellRightBorder.VerticalAlignment = Element.ALIGN_CENTER;
                bodyTable.AddCell(bodyTableCellRightBorder);

                if (total.ContainsKey(item.Uom.Unit))
                {
                    total[item.Uom.Unit] += item.Quantity;
                }
                else
                {
                    total.Add(item.Uom.Unit, item.Quantity);
                }
            }


            PdfPCell bodyTableCellFooter = new PdfPCell() { FixedHeight = 20, Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER };

            bodyTableCellFooter.Phrase = new Phrase("TOTAL  ", body_font);
            bodyTableCellFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyTableCellFooter.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellFooter.Colspan = 4;
            bodyTable.AddCell(bodyTableCellFooter);

            var val1 = total.Select(x => String.Format("{0:n0}", x.Value));
            var result1 = String.Join("\n", val1);

            var key1 = total.Select(x => String.Format("{0}", x.Key));
            var result2 = String.Join("\n", key1);

            bodyTableCellFooter.Phrase = new Phrase($"{result1}", body_font);
            bodyTableCellFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyTableCellFooter.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellFooter.Colspan = 1;
            bodyTableCellFooter.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTable.AddCell(bodyTableCellFooter);


            bodyTableCellFooter.Phrase = new Phrase($"{result2}", body_font);
            bodyTableCellFooter.HorizontalAlignment = Element.ALIGN_LEFT;
            bodyTableCellFooter.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellFooter.Border = Rectangle.RIGHT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTable.AddCell(bodyTableCellFooter);

            bodyTableCellFooter.Phrase = new Phrase("", body_font);
            bodyTableCellFooter.HorizontalAlignment = Element.ALIGN_CENTER;
            bodyTableCellFooter.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTable.AddCell(bodyTableCellFooter);

            bodyTableCellFooter.Phrase = new Phrase(totalAmount != 0 ? string.Format("{0:n2}", totalAmount) : "", body_font);
            bodyTableCellFooter.HorizontalAlignment = Element.ALIGN_RIGHT;
            bodyTableCellFooter.VerticalAlignment = Element.ALIGN_CENTER;
            bodyTableCellFooter.Border = Rectangle.RIGHT_BORDER | Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            bodyTable.AddCell(bodyTableCellFooter);

            #endregion
            bodyTable.HeaderRows = 1;
            document.Add(bodyTable);
            #endregion

            if (bank != null)
            {
                document.Add(new Paragraph("Proses Payment mohon di transfer ke   : ", normal_font));

                document.Add(new Paragraph(bank.bankName, normal_font));
                document.Add(new Paragraph(bank.bankAddress, normal_font));
                document.Add(new Paragraph("ACC NO. " + bank.AccountNumber + $"({bank.Currency.Code})", normal_font));
                document.Add(new Paragraph("A/N. PT. DAN LIRIS", normal_font));
                document.Add(new Paragraph("SWIFT CODE : " + bank.swiftCode, normal_font));
                document.Add(new Paragraph("PURPOSE CODE : 1011", normal_font));
                document.Add(new Paragraph("\n", normal_font));
            }

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        public bool IsBase64String(string base64)
        {
            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out int bytesParsed);
        }
    }

    class GarmentShippingInvoiceLocalPDFTemplatePageEvent : iTextSharp.text.pdf.PdfPageEventHelper
    {
        private GarmentShippingInvoiceViewModel viewModel;
        private int timeoffset;

        public GarmentShippingInvoiceLocalPDFTemplatePageEvent(GarmentShippingInvoiceViewModel viewModel, int timeoffset)
        {
            this.viewModel = viewModel;
            this.timeoffset = timeoffset;
        }
        public override void OnStartPage(PdfWriter writer, Document document)
        {

            PdfContentByte cb = writer.DirectContent;
            cb.BeginText();
            Font normal_font = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            Font bold_font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED);
            Font normal_font_underlined = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, Font.UNDERLINE);

            float height = writer.PageSize.Height, width = writer.PageSize.Width;
            float marginLeft = document.LeftMargin - 10, marginTop = document.TopMargin, marginRight = document.RightMargin - 10;

            cb.SetFontAndSize(bf, 8);
            
            #region table
            PdfPTable table = new PdfPTable(1);

            table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin; //this centers [table]

            PdfPTable tabledetailOrders = new PdfPTable(3);
            tabledetailOrders.SetWidths(new float[] { 0.6f, 1.4f, 2f });

            PdfPCell cellDetailContentLeft = new PdfPCell() { Border = Rectangle.TOP_BORDER };
            PdfPCell cellDetailContentRight = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER };
            PdfPCell cellDetailContentRight2 = new PdfPCell() { Border = Rectangle.BOTTOM_BORDER };
            PdfPCell cellDetailContentCenter = new PdfPCell() { Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER };
            PdfPCell cellDetailContentCenter2 = new PdfPCell() { Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER };

            PdfPCell cellHeaderContentLeft = new PdfPCell() { Border = Rectangle.NO_BORDER };
            cellHeaderContentLeft.AddElement(new Phrase("\n", normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("No. Invoice  :  " + viewModel.InvoiceNo + "                                                                           Tanggal  :  " + viewModel.InvoiceDate.ToOffset(new TimeSpan(timeoffset, 0, 0)).ToString("MMM dd, yyyy.", new System.Globalization.CultureInfo("en-EN")) + "                                                             Page  : " + (writer.PageNumber), normal_font));
            cellHeaderContentLeft.AddElement(new Phrase("\n", normal_font));
            cellHeaderContentLeft.Colspan = 3;
            tabledetailOrders.AddCell(cellHeaderContentLeft);

            cellDetailContentLeft.Phrase = new Phrase(viewModel.BuyerAgent.Name +"\n"+viewModel.ConsigneeAddress, bold_font);
            cellDetailContentLeft.Colspan = 2;
            cellDetailContentLeft.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
            tabledetailOrders.AddCell(cellDetailContentLeft);


            PdfPTable tabledetailOrders2 = new PdfPTable(3);
            tabledetailOrders2.SetWidths(new float[] { 1.5f, 0.2f, 1.5f });

            cellDetailContentLeft.Phrase = new Phrase("Mata Uang", normal_font);
            cellDetailContentLeft.Colspan = 1;
            cellDetailContentLeft.Border = Rectangle.LEFT_BORDER | Rectangle.TOP_BORDER;
            tabledetailOrders2.AddCell(cellDetailContentLeft);

            cellDetailContentLeft.Phrase = new Phrase(":", normal_font);
            cellDetailContentLeft.Colspan = 1;
            cellDetailContentLeft.Border = Rectangle.TOP_BORDER;
            tabledetailOrders2.AddCell(cellDetailContentLeft);

            cellDetailContentLeft.Phrase = new Phrase(viewModel.Items.First().CurrencyCode, normal_font);
            cellDetailContentLeft.Colspan = 1;
            cellDetailContentLeft.Border = Rectangle.TOP_BORDER;
            tabledetailOrders2.AddCell(cellDetailContentLeft);

            cellDetailContentLeft.Phrase = new Phrase("Art", normal_font);
            cellDetailContentLeft.Colspan = 1;
            cellDetailContentLeft.Border = Rectangle.LEFT_BORDER;
            tabledetailOrders2.AddCell(cellDetailContentLeft);

            cellDetailContentLeft.Phrase = new Phrase(":", normal_font);
            cellDetailContentLeft.Colspan = 1;
            cellDetailContentLeft.Border = Rectangle.NO_BORDER;
            tabledetailOrders2.AddCell(cellDetailContentLeft);

            cellDetailContentLeft.Phrase = new Phrase(viewModel.Remark, normal_font);
            cellDetailContentLeft.Colspan = 1;
            cellDetailContentLeft.Border = Rectangle.NO_BORDER;
            tabledetailOrders2.AddCell(cellDetailContentLeft);
            

            PdfPCell c2 = new PdfPCell(tabledetailOrders2);//this line made the difference
            c2.Rowspan = 3;
            tabledetailOrders.AddCell(c2);
            
            cellDetailContentRight.AddElement(new Phrase("Telp ", normal_font));
            cellDetailContentRight.Border = Rectangle.LEFT_BORDER;
            tabledetailOrders.AddCell(cellDetailContentRight);
            
            cellDetailContentCenter.AddElement(new Phrase( ": "+ viewModel.Phone, normal_font));
            cellDetailContentCenter.Border = Rectangle.NO_BORDER;
            //cellDetailContentCenter.AddElement(new Phrase(buyer.Country, normal_font));
            tabledetailOrders.AddCell(cellDetailContentCenter);

            cellDetailContentRight2.AddElement(new Phrase("Attn ", normal_font));
            cellDetailContentRight2.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
            tabledetailOrders.AddCell(cellDetailContentRight2);

            cellDetailContentCenter2.AddElement(new Phrase(": "+viewModel.Attn, normal_font));
            cellDetailContentCenter2.Border = Rectangle.BOTTOM_BORDER;
            tabledetailOrders.AddCell(cellDetailContentCenter2);

            cellDetailContentRight2.Phrase = new Phrase("\n", normal_font);
            cellDetailContentRight2.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
            tabledetailOrders.AddCell(cellDetailContentRight2);

            PdfPCell cellDetail = new PdfPCell(tabledetailOrders);
            cellDetail.Border = Rectangle.NO_BORDER;
            table.AddCell(cellDetail);

            table.WriteSelectedRows(0, -1, document.LeftMargin, height - marginTop + tabledetailOrders.TotalHeight + 10, writer.DirectContent);

            #endregion

            #region SIGNATURE
            var printY = document.BottomMargin - 100;
            var signX = document.RightMargin + 500;
            var signY = printY + 20;
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "( " + viewModel.UserAuthorizedName + " )", document.RightMargin + 500, signY + 5, 0);
            cb.MoveTo(signX - 60, signY - 2);
            cb.LineTo(signX + 45, signY - 2);
            cb.Stroke();

            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "AUTHORIZED SIGNATURE", document.RightMargin + 500, signY - 15, 0);

            #endregion

            cb.EndText();


        }
    }
}