namespace ClinicManagement.Application.Common.Interfaces
{
    public interface IHmacValidator
    {
        bool IsValid(string payload, string receivedHmac);
    }


}
