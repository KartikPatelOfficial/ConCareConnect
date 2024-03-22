using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using LaboratoryPanelWPF.Model;
using DiagnosticServicesModel.DataClass;
using iText.Html2pdf;

namespace LaboratoryPanelWPF.Utils
{

    public class TestReportGenerator
    {

        private const string APP_DATA_FOLDER_NAME = "DiagnosticServicesReports/";
        public static readonly string DEST = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"/{APP_DATA_FOLDER_NAME}/";

        #region Field Declaration

        private readonly PdfFont headerFont = PdfFontFactory.CreateFont(
            FontProgramFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD,
                iText.IO.Font.Constants.FontStyles.BOLD));

        private readonly PdfFont groupNameFont = PdfFontFactory.CreateFont(
            FontProgramFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLDOBLIQUE,
                iText.IO.Font.Constants.FontStyles.BOLD));

        private readonly PdfFont normalParameterFont = PdfFontFactory.CreateFont(
            FontProgramFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA,
                iText.IO.Font.Constants.FontStyles.NORMAL));

        private readonly PdfFont criticalParameterFont = PdfFontFactory.CreateFont(
            FontProgramFactory.CreateRegisteredFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLDOBLIQUE,
                iText.IO.Font.Constants.FontStyles.BOLD));

        private readonly float sidesMargin = Properties.Settings.Default.SidesMargin;
        private readonly float topDocumentMargin = Properties.Settings.Default.TopDocumentMargin;
        private readonly float bottomDocumentMargin = Properties.Settings.Default.BottomDocumentMargin;

        private readonly float categoryFontSize = Properties.Settings.Default.CategoryFontSize;
        private readonly float groupNameFontSize = Properties.Settings.Default.GroupNameFontSize;
        private readonly float documentFontSize = Properties.Settings.Default.DocumentFontSize;
        private readonly float resultTableHeaderFontSize = Properties.Settings.Default.ResultTableHeaderFontSize;

        #endregion

        #region PDF Genrate Initials
        public string GenerateReport(List<Test> dataList, Appointment appointment)
        {
            //var dialog = new TestGroupSelectionDialog() { DataContext = new TestGroupSelectionViewModel(dataList) };
            //var testResults = (List<TestResultCategory>)await DialogHost.Show(dialog, "RootDialog");

            //if (testResults == null) return null;

            var fileDestination = DEST + $@"{appointment.Patient.Name}_{appointment.Patient.Id}.pdf";
            var file = new FileInfo(fileDestination);
            file.Directory.Create();

            ManipulatePdf(dataList, fileDestination, appointment);

            return fileDestination;
        }

        public string GenerateReport(List<Test> dataList, Appointment appointment, List<TestHistoryResponse> testHistoryResponse, string sourcePdf)
        {
            var genratedPdf = GenerateReport(dataList, appointment);
            var newDestination = genratedPdf.Replace(".pdf", "_merged.pdf");

            PdfDocument pdf = new PdfDocument(new PdfWriter(newDestination));
            PdfMerger merger = new PdfMerger(pdf);

            //Add pages from the first document
            PdfDocument firstSourcePdf = new(new PdfReader(genratedPdf));
            merger.Merge(firstSourcePdf, 1, firstSourcePdf.GetNumberOfPages());

            //Add pages from the second pdf document
            PdfDocument secondSourcePdf = new(new PdfReader(sourcePdf));
            merger.Merge(secondSourcePdf, 1, secondSourcePdf.GetNumberOfPages());

            firstSourcePdf.Close();
            secondSourcePdf.Close();
            merger.Close();
            pdf.Close();

            return newDestination;
        }
        #endregion

        private void ManipulatePdf(List<Test> dataList, string pdfDest, Appointment appointment)
        {
            var writer = new PdfWriter(pdfDest);
            var pdfDocument = new PdfDocument(writer);
            var document = new iText.Layout.Document(pdfDocument);
            var headerHandler = new Header(appointment);

            document.SetMargins(topDocumentMargin, sidesMargin, bottomDocumentMargin, sidesMargin);

            pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);

            pdfDocument.AddNewPage();

            for (var i = 0; i < dataList.Count; i++)
            {
                var paragraph = new Paragraph(dataList[i].Name);
                paragraph.SetFont(headerFont);
                paragraph.SetTextAlignment(TextAlignment.CENTER);
                paragraph.SetFontSize(categoryFontSize);
                document.Add(paragraph);

                var table = new Table(4).UseAllAvailableWidth();

                //Test Result Table Header
                table.AddHeaderCell("TEST NAME");
                table.AddHeaderCell("RESULT");
                table.AddHeaderCell("UNITS");
                table.AddHeaderCell("NORMAL RANGE");

                var tableHeaderStyle = new Style();
                tableHeaderStyle.SetFontSize(resultTableHeaderFontSize);

                table.GetHeader().SetBorder(Border.NO_BORDER);
                table.GetHeader().SetFontSize(resultTableHeaderFontSize);
                table.GetHeader().AddStyle(tableHeaderStyle);


                //Test Result Table
                var tableStyle = new Style();
                tableStyle.SetFontSize(documentFontSize);
                tableStyle.SetBorder(Border.NO_BORDER);

                var data = dataList[i].Parameters.GroupBy(r => r.GroupName)
                    .ToDictionary(t => t.Key, t => t.Select(r => r).ToList());

                var categoryId = dataList[i].Id;

                foreach (var groups in data)
                {
                    var groupNameCell = new Cell(0, 4);
                    groupNameCell.Add(new Paragraph(groups.Key.ToUpper()));
                    groupNameCell.SetFont(groupNameFont);
                    groupNameCell.SetFontSize(groupNameFontSize);
                    groupNameCell.SetUnderline();
                    groupNameCell.SetPaddingTop(16);

                    table.AddCell(groupNameCell);

                    groups.Value.ForEach(test =>
                    {
                        var font = test.IsCritical ? criticalParameterFont : normalParameterFont;
                        var result = test.Value;

                        if (!string.IsNullOrEmpty(result))
                        {
                            result = test.Value + (test.IsCritical ? "\tH" : "\tL");
                        }


                        table.AddCell(new Paragraph(test.Name).SetFont(normalParameterFont).SetFontSize(documentFontSize));
                        table.AddCell(new Paragraph(result).SetFont(font).SetFontSize(documentFontSize));
                        table.AddCell(new Paragraph(test.Unit).SetFont(normalParameterFont).SetFontSize(documentFontSize));
                        table.AddCell(new Paragraph(test.Normalrange ?? "").SetFont(normalParameterFont).SetFontSize(documentFontSize));


                    });
                }


                foreach (var element in table.GetChildren())
                {
                    ((Cell)element).SetBorder(Border.NO_BORDER);
                }

                table.AddStyle(tableStyle);
                document.Add(table);

                //Category Note
                var categoryNote = dataList[i].Categorynote;
                if (categoryNote != null && categoryNote != "" && categoryNote.Trim() != "-")
                {
                    document.Add(new Paragraph());
                    var html = RtfPipe.Rtf.ToHtml(categoryNote);
                    var elements = HtmlConverter.ConvertToElements(html);
                    foreach (IElement element in elements)
                    {
                        document.Add((IBlockElement)element);
                    }
                }

                if (i != dataList.Count - 1)
                {
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }
            }


            pdfDocument.Close();
            document.Close();
        }

        // Header event handler
        protected class Header : IEventHandler
        {
            private readonly string id;
            private readonly string name;
            private readonly string ageSex;
            private readonly string referredBy;
            private readonly string regDate;
            private readonly string ward;
            private readonly string reportedTime;
            private readonly string sampleType;

            private readonly float topHeaderMargin = Properties.Settings.Default.TopHeaderMargin;
            private readonly float sidesMargin = Properties.Settings.Default.SidesMargin;
            private readonly float headerFontSide = Properties.Settings.Default.HeaderTableFontSize;

            public Header(Appointment appointmentResponse)
            {

                id = appointmentResponse.Patient.Id.ToString();
                name = appointmentResponse.Patient.Name;
                ageSex = " | " + appointmentResponse.Patient.Gender;
                referredBy = appointmentResponse.Doctor.Name;
                regDate = appointmentResponse.Createdat!.Value.ToString("g");
                ward = appointmentResponse.Patienttype ?? "IDP";
                reportedTime = regDate;
                sampleType = appointmentResponse.Sampletype ?? "-/-";
            }

            public virtual void HandleEvent(Event @event)
            {
                string backgroundImage = Properties.Settings.Default.BackgroudImagePath;

                var docEvent = (PdfDocumentEvent)@event;

                var page = docEvent.GetPage();
                var pageSize = page.GetPageSize();

                var pdfCanvas = new PdfCanvas(page);
                if (backgroundImage != null && backgroundImage != "")
                {
                    pdfCanvas.AddImageFittedIntoRectangle(ImageDataFactory.Create(backgroundImage), pageSize, false);
                }

                var canvas = new Canvas(pdfCanvas, pageSize);
                canvas.SetFontSize(headerFontSide);

                var table = new Table(new float[] { 2, 3, 2, 3 }).UseAllAvailableWidth();
                table.SetMarginLeft(sidesMargin);
                table.SetMarginRight(sidesMargin);
                table.SetMarginTop(topHeaderMargin);

                table.AddCell("Name".ToUpper());
                table.AddCell(name.ToUpper());

                table.AddCell("Ward".ToUpper());
                table.AddCell(ward.ToUpper());

                table.AddCell("Age & Sex".ToUpper());
                table.AddCell(ageSex.ToUpper());

                table.AddCell("Reg Date".ToUpper());
                table.AddCell(regDate.ToUpper());

                table.AddCell("Referred By".ToUpper());
                table.AddCell(referredBy.ToUpper());

                table.AddCell("Collected On".ToUpper());
                table.AddCell(reportedTime.ToUpper());

                table.AddCell("Id".ToUpper());
                table.AddCell(id);

                table.AddCell("Sample Type".ToUpper());
                table.AddCell(sampleType);


                foreach (Cell element in table.GetChildren())
                {
                    element.SetBorder(Border.NO_BORDER);
                    if (element.GetCol() % 2 == 0)
                    {
                        element.SetBold();
                    }
                }

                canvas.Add(table);
                canvas.Close();
            }
        }
    }
}