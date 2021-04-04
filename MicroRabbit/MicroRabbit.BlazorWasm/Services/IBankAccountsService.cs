using MicroRabbit.BlazorWasm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.BlazorWasm.Services
{
    interface IBankAccountsService
    {
        public IList<BankAccount> BankAccounts { get; set; }
        public Task<IList<BankAccount>> GetBankAccounts();
    }
}
