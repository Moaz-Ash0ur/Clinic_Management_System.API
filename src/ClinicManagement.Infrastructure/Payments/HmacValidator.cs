using ClinicManagement.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace ClinicManagement.Infrastructure.Payments
{
    public class HmacValidator : IHmacValidator
    {
        private readonly string _secret;

        public HmacValidator(IConfiguration config)
        {
            _secret = config["Paymob:HMAC"]!;
        }

        public bool IsValid(string data, string receivedHmac)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

            var sb = new StringBuilder(hash.Length * 2);

            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            var calculated = sb.ToString();

            return calculated == receivedHmac.ToLower();

        }
    }


}
