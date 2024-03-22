using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using LaboratoryPanelWPF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using DiagnosticServicesModel.DataClass;
using DiagnosticServicesModel.Request;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Properties;

namespace LaboratoryPanelWPF.Utils
{
    public class InvoiceGenerator
    {
        private const string APP_DATA_FOLDER_NAME = "DiagnosticServicesInvoices/";

        public static readonly string DEST = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                             $"/{APP_DATA_FOLDER_NAME}/";

        public static string GenerateInvoice(Invoice invoice, Patient patient)
        {
            var fileName = $"{invoice.AppointmentRequest.Patientid}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
            var outputPath = DEST + fileName;

            // Initialize PDF writer
            var writer = new PdfWriter(outputPath);
            // Initialize PDF document
            var pdf = new PdfDocument(writer);
            // Initialize document
            var document = new Document(pdf);

        // Optionally, add a company logo
        var logoPath = "path/to/your/logo.png"; // Make sure to have a valid path for your logo
        if (File.Exists(logoPath))
        {
            ImageData imageData = ImageDataFactory.Create(logoPath);
            Image logo = new Image(imageData).SetWidth(50).SetHeight(50);
            document.Add(logo);
        }

        // Add a line separator
        LineSeparator ls = new LineSeparator(new SolidLine());
        document.Add(ls);

        // Title
        document.Add(new Paragraph("ConCare Connect Invoice")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20)
            .SetBold()
            .SetUnderline());

        // Patient Details
        AddSectionHeader(document, "Patient Details");
        AddDetail(document, "Patient Name:", patient.Name);
        AddDetail(document, "Patient ID:", patient.Id.ToString());
        AddDetail(document, "Patient Email:", patient.Email);

        // Appointment Details
        AddSectionHeader(document, "Appointment Details");
        AddDetail(document, "Appointment ID:", invoice.AppointmentRequest.Patientid.ToString());
        AddDetail(document, "Doctor ID:", invoice.AppointmentRequest.Doctorid.ToString());
        AddDetail(document, "Date:", DateTime.Now.ToString("yyyy-MM-dd"));

        // Invoice Details
        AddSectionHeader(document, "Invoice Details");
        AddDetail(document, "Total Price:", $"{invoice.TotalPrice:C}");
        AddDetail(document, "Discount:", $"{invoice.Discount:C}");
        AddDetail(document, "Price After Discount:", $"{invoice.PriceAfterDiscount:C}");

        // Sample Type & Tests Details
        AddSectionHeader(document, "Sample and Tests Details");
        AddDetail(document, "Sample Type:", invoice.AppointmentRequest.Sampletype);
        AddTestsDetails(document, invoice.AppointmentRequest.Tests);

        // Payment Status
        AddDetail(document, "Payment Status:", invoice.AppointmentRequest.Paymentstatus);


            // Close document
            document.Close();

            return outputPath;
        }

        private static void AddSectionHeader(Document document, string text)
        {
            document.Add(new Paragraph(text)
                .SetFontSize(16)
                .SetBold()
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY, 0.5f)
                .SetMarginBottom(10));
        }

        private static void AddDetail(Document document, string label, string value)
        {
            document.Add(new Paragraph($"{label} {value}")
                .SetFontSize(12)
                .SetMarginBottom(5));
        }

        private static void AddTestsDetails(Document document, List<TestRequest> tests)
        {
            if (tests.Count <= 0) return;
            foreach (var test in tests)
            {
                AddDetail(document, $"{test.TestName} - Cost:", $"{test.TestPrice:C}");
            }
        }
    }
}