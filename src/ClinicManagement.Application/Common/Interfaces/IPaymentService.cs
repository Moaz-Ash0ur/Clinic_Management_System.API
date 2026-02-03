using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Common.Interfaces
{

    public interface IPayment
    {
        Task<string> GetAccessTokenAsync();
        Task<string> CreateOrderAsync();
        Task<string> CreatePaymentKeyAsync();
    }

}
