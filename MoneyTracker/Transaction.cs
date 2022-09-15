using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker
{
    public  class Transaction
    {
        public string Title { get; set; }
        public int Value { get; set; }
    }

    class Expense : Transaction
    {
        public Expense (string title, int value)
        {
            Title = title;
            Value = value;
        }
    }
    class Income : Transaction
    {
        public Income (string title, int value)
        {
            Title = title;
            Value = value;
        }
    }
}
