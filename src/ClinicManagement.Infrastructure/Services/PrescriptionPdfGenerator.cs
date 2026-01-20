using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Prescriptions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace ClinicManagement.Infrastructure.Services
{
    public sealed class PrescriptionPdfGenerator : IPrescriptionPdfGenerator
    {
        public byte[] Generate(Prescription prescription)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Header().Element(BuildHeader(prescription));
                    page.Content().Element(BuildContent(prescription));
                    page.Footer().Element(BuildFooter());
                });
            }).GeneratePdf();
        }

        // ================= HEADER =================
        private Action<IContainer> BuildHeader(Prescription prescription) => header =>
        {
            header.Column(col =>
            {
                col.Item().Row(row =>
                {
                    // Clinic Logo / Icon
                    row.ConstantItem(50)
                        .Height(50)
                        .Background(Colors.Teal.Medium)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text("🩺")
                        .FontSize(26)
                        .FontColor(Colors.White);

                    // Clinic Name
                    row.RelativeItem().PaddingLeft(15).AlignMiddle().Text(text =>
                    {
                        text.Span("Al Shifa Medical Clinic\n")
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Teal.Darken2);

                        text.Span("Medical Prescription")
                            .FontSize(12)
                            .FontColor(Colors.Grey.Medium);
                    });

                    // Prescription Meta
                    row.RelativeItem().AlignRight().Column(meta =>
                    {
                        meta.Item().Text($"Prescription # {prescription.Id.ToString()[..8]}")
                            .FontSize(12)
                            .Bold();

                        meta.Item().Text($"Date: {prescription.CreatedAtUtc:dd MMM yyyy}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);
                    });
                });

                col.Item().PaddingVertical(15)
                    .LineHorizontal(2)
                    .LineColor(Colors.Teal.Lighten2);
            });
        };

        // ================= CONTENT =================
        private Action<IContainer> BuildContent(Prescription prescription) => content =>
        {
            content.Column(col =>
            {
                // Patient Info
                col.Item().PaddingBottom(20).Border(1).BorderColor(Colors.Grey.Lighten2).Padding(10)
                    .Column(info =>
                    {
                        info.Item().Text("Patient Information")
                            .Bold()
                            .FontColor(Colors.Teal.Darken2);

                        info.Item().Text($"Patient Name: {prescription.Patient.Id}");
                        info.Item().Text($"Session ID: {prescription.SessionId}");
                    });

                // Medication Table
                col.Item().Table(table =>
                {
                    table.ColumnsDefinition(c =>
                    {
                        c.RelativeColumn(3); // Medication
                        c.RelativeColumn(2); // Dosage
                        c.RelativeColumn(5); // Description
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Teal.Medium).Padding(8)
                            .Text("Medication").Bold().FontColor(Colors.White);

                        header.Cell().Background(Colors.Teal.Medium).Padding(8)
                            .Text("Dosage").Bold().FontColor(Colors.White);

                        header.Cell().Background(Colors.Teal.Medium).Padding(8)
                            .Text("Instructions").Bold().FontColor(Colors.White);
                    });

                    table.Cell().Padding(8).Text(prescription.MedicationName);
                    table.Cell().Padding(8).Text(prescription.Dosage);
                    table.Cell().Padding(8).Text(prescription.Description);
                });

                // Doctor Signature
                col.Item().PaddingTop(30).AlignRight().Column(sign =>
                {
                    sign.Item().Text("________________________");
                    sign.Item().Text("Doctor Signature")
                        .FontSize(10)
                        .FontColor(Colors.Grey.Medium);
                });
            });
        };

        // ================= FOOTER =================
        private Action<IContainer> BuildFooter() => footer =>
        {
            footer.AlignCenter().Text(text =>
            {
                text.Span("This prescription is generated electronically • ")
                    .FontSize(9)
                    .FontColor(Colors.Grey.Medium);

                text.Span($"{DateTime.UtcNow:dd MMM yyyy HH:mm} UTC")
                    .FontSize(9)
                    .FontColor(Colors.Grey.Medium);
            });
        };
    }

}
