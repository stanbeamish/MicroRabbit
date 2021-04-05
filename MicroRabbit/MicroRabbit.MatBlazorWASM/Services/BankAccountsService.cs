
using MicroRabbit.MatBlazorWASM.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroRabbit.MatBlazorWASM.Services
{
    public class BankAccountsService : IBankAccountsService
    {
        private readonly HttpClient _httpClient;

        public BankAccountsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IList<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

        public async Task<IList<BankAccount>> GetBankAccounts()
        {
            BankAccounts = await _httpClient.GetJsonAsync<BankAccount[]>("https://localhost:5001/api/Banking");
            return BankAccounts;
        }       
    }
}
