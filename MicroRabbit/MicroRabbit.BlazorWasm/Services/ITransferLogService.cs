
using MicroRabbit.BlazorWasm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroRabbit.BlazorWasm.Services
{
    interface ITransferLogService
    {
        public IList<TransferLog> TransferLogs { get; set; }
        public Task<IList<TransferLog>> GetTransferLogs();
    }
}
