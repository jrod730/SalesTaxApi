using Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Engines
{
    public interface IReceiptEngine
    {
        Task<string> GenerateReceiptAsync(IEnumerable<ReceiptRequest> receiptRequests);
    }
}
