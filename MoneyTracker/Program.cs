using MoneyTracker;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;


/*
 * Project Brief 
 * Your task is to build a money tracking application.
 * The application will allow a user to enter expense(s) and income(s) to the application. 
 * Further, they should be able to assign a month to an expense or income. 
 * They will interact with a text based user interface via the command-line. 
 * Once they are using the application, the user should be able to also edit and remove items from the application. 
 * They can also quit and save the current task list to file, and then restart the application with the former state restored. 
 * The interface should look similar to the mockup below: 
 * 
 *      >> Welcome to TrackMoney
 *      >> You currently have (X) SEK on your account.
 *      >> Pick an option:
 *      >> (1) Show items (All/Expenses/Income)
 *      >> (2) Add New Expense/Income
 *      >> (3) Edit Item (edit, remove)
 *      >> (4) Save and Quit
 *      
 * Requirements 
 *    The solution must achieve the following requirements:
 *      - Model an item with title, amount,and month. (Solve the problem where you have to distinguish income and expense) 
 *      - Display a collection of items that can be sorted in ascending or descending order. Sorted by month, amount or title. 
 *      - Display only expenses or only incomes. 
 *      - Support the ability to edit, and remove items 
 *      - Support a text-based user interface 
 *      - Load and save items list to file 
 *    The solution may also include other creative features at your discretion in case you wish to show some flair. 
 *    
 *    X Disallow empty titles on transactions
 *    
 *    TODO:
 *      - Testing
 *      
 *    
 */


// Initiate List of transactions
List<Transaction> transactions = new List<Transaction>();

// Welcome greeting
Console.WriteLine();
Console.ForegroundColor = ConsoleColor.DarkBlue;
Console.WriteLine("                   ***    Welcome to TrackMoney    ***                   ");
Console.ResetColor();
Console.WriteLine();

Main(transactions);

static void Main(List<Transaction> transactions)
{
    // Print account balance and instructions
    Console.WriteLine("--------------------------------------------------------------------------");
    Console.WriteLine();
    Console.Write("You currently have ");
    Console.ForegroundColor = ConsoleColor.DarkBlue;
    Console.Write($"{Balance(transactions)}");
    Console.ResetColor();
    Console.Write(" SEK on your account.");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Pick an option:");
    Console.WriteLine();
    Console.WriteLine("(1) Show items (All/Expenses/Incomes)");
    Console.WriteLine("(2) Add New Expense/Income");
    Console.WriteLine("(3) Edit Item (edit, remove)");
    Console.WriteLine("(4) Save and Quit");
    Console.WriteLine();

    string option = Console.ReadLine();

    // Menu options calling appropriate method
    switch (option)
    {
        case "1":
            Console.WriteLine();
            Console.WriteLine("Choose which items You would like to display:");
            Console.WriteLine();
            Console.WriteLine("(1) All");
            Console.WriteLine("(2) Expense(s)");
            Console.WriteLine("(3) Income(s)");
            Console.WriteLine("(4) Return to Main Menu");
            Console.WriteLine();

            string showOption = Console.ReadLine();
            switch (showOption)
            {
                case "1":
                    ShowItems(transactions, "all");
                    Main(transactions);
                    break;
                case "2":
                    ShowItems(transactions, "expenses");
                    Main(transactions);
                    break;
                case "3":
                    ShowItems(transactions, "incomes");
                    Main(transactions);
                    break;
                case "4":                    
                    break;
                default:
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong input");
                    Console.ResetColor();
                    Console.WriteLine();
                    Main(transactions);
                    break;
            }
            break;
        case "2":
            Console.WriteLine();
            Console.WriteLine("Choose what You would like to add:");
            Console.WriteLine();
            Console.WriteLine("(1) Expense");
            Console.WriteLine("(2) Income");
            Console.WriteLine("(3) Return to Main Menu");
            Console.WriteLine();

            string addOption = Console.ReadLine();
            switch (addOption)
            {
                case "1":
                    AddNew(transactions, "expense");
                    Main(transactions);
                    break;
                case "2":
                    AddNew(transactions, "income");
                    Main(transactions);
                    break;
                case "3":
                    Main(transactions);
                    break;
                default:
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong input");
                    Console.ResetColor();
                    Console.WriteLine();
                    Main(transactions);
                    break;
            }
            break;
        case "3":
            Console.WriteLine();
            Console.WriteLine("Choose what You would like to do:");
            Console.WriteLine();
            Console.WriteLine("(1) Edit");
            Console.WriteLine("(2) Remove");
            Console.WriteLine("(3) Return to Main Menu");
            Console.WriteLine();

            string editOption = Console.ReadLine();
            switch (editOption)
            {
                case "1":
                    EditItem(transactions, "edit");
                    Main(transactions);
                    break;
                case "2":
                    EditItem(transactions, "remove");
                    Main(transactions);
                    break;
                case "3":
                    Main(transactions);
                    break;
                default:
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong input");
                    Console.ResetColor();
                    Console.WriteLine();
                    Main(transactions);
                    break;
            }
            break;
        case "4":
            Main(transactions);
            break;
        default:
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input");
            Console.ResetColor();
            Console.WriteLine();
            Main(transactions);
            break;
    }
}

// Method to print transactions
static void ShowItems(List<Transaction> transactions, string items)
{
    int tCount = 0;

    if (transactions.Count() < 1)
    {
        Console.WriteLine();
        Console.WriteLine(" --- No transactions to display ---");
        Console.WriteLine();
    }

    // Print all transactions
    else if (items == "all")
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("--- ALL TRANSACTIONS ---".PadLeft(50));
        Console.WriteLine("--------------------------------------------------------------------------");
        Console.WriteLine("#:".PadLeft(10) + " - " + "Title:".PadRight(20) + "Type:".PadRight(20) + "Value:");
        Console.ResetColor();
        Console.WriteLine();

        // Check each transaction's type and print appropriate data to table
        foreach (Transaction t in transactions)
        {
            // Check if transation's type is expense
            if(t.GetType() == typeof(Expense))
            {
                // Convert the expense's value to negative and to string
                string value = (t.Value * -1).ToString();

                // Print expense
                Console.WriteLine(tCount.ToString().PadLeft(10) + " - " + t.Title.PadRight(20) + "EXPENSE".PadRight(20) + value + " SEK");
            }

            // Check if transation's type is income
            else if (t.GetType() == typeof(Income))
            {
                // Convert the income's value to string
                string value = t.Value.ToString();

                // Print income
                Console.WriteLine(tCount.ToString().PadLeft(10) + " - " + t.Title.PadRight(20) + "INCOME".PadRight(20) + value + " SEK");
            }
            tCount++;
        }
    }

    // Print expenses in a table
    else if (items == "expenses")
    {
        // Check if there are expenses in list
        bool empty = true;
        foreach (Transaction t in transactions)
        {
            if (t.GetType() == typeof(Expense))
            {
                empty = false;
            }
        }

        // Print incomes if they exist
        if (empty == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("--- EXPENSES ---".PadLeft(50));
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("#:".PadLeft(10) + " - " + "Title:".PadRight(20) + "Value:");
            Console.ResetColor();
            Console.WriteLine();

            // Check each transaction's type and print appropriate data to table
            foreach (Transaction t in transactions)
            {
                // Check if transation's type is expense
                if (t.GetType() == typeof(Expense))
                {
                    // Convert the expense's value to negative and to string
                    string value = (t.Value * -1).ToString();

                    // Print expense
                    Console.WriteLine(tCount.ToString().PadLeft(10) + " - " + t.Title.PadRight(20) + value + " SEK");
                }
                tCount++;
            }
        }
        
        // Print message if there are no expenses
        else
        {
            Console.WriteLine();
            Console.WriteLine(" --- No expenses to display ---");
            Console.WriteLine();

        }
    }

    // Print incomes in a table
    else if (items == "incomes")
    {
        // Check if there are incomes in list
        bool empty = true;
        foreach (Transaction t in transactions)
        {
            if (t.GetType() == typeof(Income))
            {
                empty = false;
            }
        }

        // Print incomes if they exist
        if (empty == false)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("--- INCOMES ---".PadLeft(50));
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("#:".PadLeft(10) + " - " + "Title:".PadRight(20) + "Value:");
            Console.ResetColor();
            Console.WriteLine();

            foreach (Transaction t in transactions)
            {
                if (t.GetType() == typeof(Income))
                {
                    // Convert the income's value to string
                    string value = t.Value.ToString();

                    // Print income
                    Console.WriteLine(tCount.ToString().PadLeft(10) + " - " + t.Title.PadRight(20) + value + " SEK");
                }
                tCount++;
            }
        }

        // Print message if there are no incomes
        else
        {
            Console.WriteLine();
            Console.WriteLine(" --- No incomes to display ---");
            Console.WriteLine();

        }
    }
    Console.WriteLine();
}

// Method to display account balance
static int Balance(List<Transaction> transactions)
{
    int incomes = 0;
    int expenses = 0;

    // Return 0 if there are no transactions
    if (transactions.Count() < 1)
    {
        return 0;
    }
    
    // Check type of each transaction in list
    for (int i = 0; i < transactions.Count(); i++)
    {
        // Subtract each expense 
        if (transactions[i] is Expense)
        {
            expenses += transactions[i].Value;
        }

        // Add each income
        else if (transactions[i] is Income)
        {
            incomes += transactions[i].Value;
        }
    }
    
    // Subtract expenses from incomes and return result
    return incomes - expenses;
}

// Method to add new transactions
static void AddNew(List<Transaction> transactions, string type)
{
    // Prompt user for title and amount of the transaction
    Console.WriteLine();
    Console.WriteLine($"Enter a title for this {type}:");
    Console.WriteLine();

    string title = Console.ReadLine();

    Console.WriteLine();
    Console.WriteLine($"Enter the amount of this {type}:");
    Console.WriteLine();

    string sValue = Console.ReadLine();

    // Attempt to convert the amount inputted by the user from string to int
    try
    {
        int value = Convert.ToInt32(sValue);

        // Convert value from negative to positive if needed
        if (value < 0)
        {
            value = value * -1;
        }
        
        // Make sure title is not empty
        if (title.Trim() != "")
        {
            // Check if transaction is an expense or income
            if (type == "expense")
            {
                // Add expense to transactions list
                transactions.Add(new Expense(title, value));
            }
            else if (type == "income")
            {
                // Add income to transactions list
                transactions.Add(new Income(title, value));
            }
        }

        // Print error if title is empty
        else
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong input, you must enter a title");
            Console.ResetColor();
            Console.WriteLine();
        }        
    }

    // Catch if number is to big for int and print error
    catch (OverflowException)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The amount is to big, must be of type Integer.");
        Console.ResetColor();
        Console.WriteLine();
    }

    // Catch remaining errors and provide correct usage instuctions
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The amount must be a positive number of the type Integer");
        Console.ResetColor();
        Console.WriteLine();
    }
}

// Method to edit or delete transactions
static void EditItem(List<Transaction> transactions, string action)
{
    // Print list of transations
    ShowItems(transactions, "all");

    Console.WriteLine();
    Console.Write($"Enter the number corresponding to the transaction you would like to {action}:");
    Console.WriteLine();

    string input = Console.ReadLine();

    try
    {
        // Point to user-selected transaction
        Transaction selectedItem = transactions[int.Parse(input)];
        
        // Edit transaction
        if (action == "edit")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Are you sure you would like to edit {selectedItem.Title}?");            
            Console.WriteLine("Enter 'Y' or 'YES' to confirm");
            Console.ResetColor();
            Console.WriteLine();

            string confirm = Console.ReadLine();
            if (confirm == "y".ToLower().Trim() || confirm == "yes".ToLower().Trim())
            {
                Console.WriteLine();
                Console.WriteLine("Enter the new title for transaction:");
                Console.WriteLine();
                string title = Console.ReadLine();

                // Update the selected item's title
                selectedItem.Title = title;

                Console.WriteLine();
                Console.WriteLine("Enter the new amount of the transaction (must be positve number):");
                Console.WriteLine();
                string sValue = Console.ReadLine();

                // Attempt to convert the amount inputted by the user from string to int
                try
                {
                    int value = Convert.ToInt32(sValue);

                    // Convert value from negative to positive if needed
                    if (value < 0)
                    {
                        value = value * -1;
                    }

                    // Update the selected item's value
                    selectedItem.Value = value;
                }

                // Catch if number is to big for int and print error
                catch (OverflowException)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The amount is to big, must be of type Integer.");
                    Console.ResetColor();
                    Console.WriteLine();
                }

                // Catch remaining errors and provide correct usage instuctions
                catch (Exception ex)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The amount must be a positive number of the type Integer");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }
        }

        // Confirm and remove transaction
        else if (action == "remove")
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"Are you sure you would like to remove {selectedItem.Title}?");
            Console.WriteLine("Enter 'Y' or 'YES' to confirm");
            Console.ResetColor();
            Console.WriteLine();

            string confirm = Console.ReadLine();

            if (confirm == "y".ToLower().Trim() || confirm == "yes".ToLower().Trim())
            {
                transactions.Remove(selectedItem);
            }
        }
    }

    // Print error if input is wrong
    catch
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Wrong input, your input does not correspond to any transaction");
        Console.ResetColor();
        Console.WriteLine();
        return;
    }    
}

