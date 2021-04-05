using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static System.Console;
using MicroRabbit.MatBlazorWASM.Models;

namespace MicroRabbit.MatBlazorWASM.Services
{
    public class TransferLogService : ITransferLogService
    {
        private readonly HttpClient _httpClient;

        public TransferLogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IList<TransferLog> TransferLogs { get; set; } = new List<TransferLog>();

        public async Task<IList<TransferLog>> GetTransferLogs()
        {
            TransferLogs = await _httpClient.GetJsonAsync<TransferLog[]>("https://localhost:5003/api/Transfer");
            return TransferLogs;
        }        
    }
}
