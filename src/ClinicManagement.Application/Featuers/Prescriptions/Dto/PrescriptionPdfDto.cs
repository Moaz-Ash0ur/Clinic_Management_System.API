namespace ClinicManagement.Application.Featuers.Prescriptions.Dto

{
    public sealed class PrescriptionPdfDto
    {
        public byte[] Content { get; set; } = default!;
        public string FileName { get; set; } = string.Empty;
    }


}
