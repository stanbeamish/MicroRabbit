using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.MatBlazorWASM.Models
{
    public class BankAccount
    {
        public int ID { get; set; }
        public string AccountType { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
