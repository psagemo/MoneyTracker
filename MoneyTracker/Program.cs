using MoneyTracker;
using System.Diagnostics;
using System.Runtime.InteropServices;


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
 *    TODO:
 *      - Testing
 *      - Disallow empty titles on transations
 *    
 */


// Initiate List of transactions
List<Transaction> transactions = new List<Transaction>();

Main(transactions);

static void Main(List<Transaction> transactions)
{
    

    // Greeting to user with account balance and instructions
    Console.WriteLine("Welcome to TrackMoney");
    Console.WriteLine($"You currently have {Balance(transactions)} SEK on your account.");
    Console.WriteLine("Pick an option:");
    Console.WriteLine("(1) Show items (All/Expenses/Incomes)");
    Console.WriteLine("(2) Add New Expense/Income");
    Console.WriteLine("(3) Edit Item (edit, remove)");
    Console.WriteLine("(4) Save and Quit");

    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            Console.WriteLine("Choose which items You would like to display:");
            Console.WriteLine("(1) All");
            Console.WriteLine("(2) Expense(s)");
            Console.WriteLine("(3) Income(s)");
            Console.WriteLine("(4) Return to Main Menu");

            string showOption = Console.ReadLine();
            switch (showOption)
            {
                case "1":
                    ShowItems(transactions, "all");
                    break;
                case "2":
                    ShowItems(transactions, "expenses");
                    break;
                case "3":
                    ShowItems(transactions, "incomes");
                    break;
                case "4":                    
                    break;
            }
            break;
        case "2":
            Console.WriteLine("Choose what You would like to add:");
            Console.WriteLine("(1) Expense");
            Console.WriteLine("(2) Income");
            Console.WriteLine("(3) Return to Main Menu");

            string addOption = Console.ReadLine();
            switch (addOption)
            {
                case "1":
                    AddNew(transactions, "expense");
                    break;
                case "2":
                    AddNew(transactions, "income");
                    break;
                case "3":
                    break;
            }
            break;
        case "3":
            Console.WriteLine("Choose what You would like to do:");
            Console.WriteLine("(1) Edit");
            Console.WriteLine("(2) Remove");
            Console.WriteLine("(3) Return to Main Menu");

            string editOption = Console.ReadLine();
            switch (editOption)
            {
                case "1":
                    EditItem(transactions, "edit");
                    break;
                case "2":
                    EditItem(transactions, "remove");
                    break;
                case "3":
                    Main(transactions);
                    break;
            }
            break;
        case "4":
            break;
    }
}

static void ShowItems(List<Transaction> transactions, string items)
{
    if (items == "all")
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("EXPENSES");
        Console.WriteLine();
        Console.WriteLine("Title:".PadRight(30) + "Type:".PadRight(30) + "Value:");
        Console.ResetColor();

        // Check each transaction's type and print appropriate data to table
        foreach (Transaction t in transactions)
        {
            // Check if transation's type is expense
            if(t.GetType() == typeof(Expense))
            {
                // Convert the expense's value to negative and to string
                string value = (t.Value * -1).ToString();

                // Print expense
                Console.WriteLine(t.Title.PadRight(30) + "EXPENSE".PadRight(30) + value + " SEK");
            }

            // Check if transation's type is income
            else if (t.GetType() == typeof(Income))
            {
                // Convert the income's value to string
                string value = t.Value.ToString();

                // Print income
                Console.WriteLine(t.Title.PadRight(30) + "INCOME".PadRight(30) + value + " SEK");
            }
        }
    }

    // Print expenses in a table
    else if (items == "expenses")
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("EXPENSES");
        Console.WriteLine();
        Console.WriteLine("Title:".PadRight(30) + "Value:");
        Console.ResetColor();
        foreach (Expense e in transactions)
        {
            // Convert the expenses value to negative and to string
            string value = (e.Value * -1).ToString();
            
            // Print expense
            Console.WriteLine(e.Title.PadRight(30) + value + " SEK");
        }
    }
    else if (items == "incomes")
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("INCOMES");
        Console.WriteLine();
        Console.WriteLine("Title:".PadRight(30) + "Value:");
        Console.ResetColor();
        foreach (Income i in transactions)
        {
            // Convert the incomes value to string
            string value = i.Value.ToString();

            // Print income
            Console.WriteLine(i.Title.PadRight(30) + value + " SEK");
        }
    }
    
}

static int Balance(List<Transaction> transactions)
{
    int incomes = 0;
    int expenses = 0;

    // Subtract each expense 
    foreach (Expense expense in transactions)
    {
        expenses -= expense.Value;
    }

    // Add each income
    foreach (Income income in transactions)
    {
        incomes += income.Value;
    }
    
    // Subtract expenses from incomes and return result
    return incomes - expenses;
}

static void AddNew(List<Transaction> transactions, string type)
{
    // Prompt user for title and amount of the transaction
    Console.WriteLine($"Enter a title for this {type}:");
    string title = Console.ReadLine();
    Console.WriteLine($"Enter the amount of this {type}:");
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

    // Catch if number is to big for int and print error
    catch (OverflowException)
    {
        Console.WriteLine("The amount is to big, must be of type Integer.");
    }

    // Catch remaining errors and provide correct usage instuctions
    catch (Exception ex)
    {
        Console.WriteLine("The amount must be a positive number of the type Integer");
    }
}

static void EditItem(List<Transaction> transactions, string action)
{
    Console.Write($"Enter the title of the transaction you would like to {action} (or enter 'S' or 'SHOW' in order to view all transactions): ");
    string input = Console.ReadLine();

    if (input.ToLower().Trim() == "s" || input.ToLower().Trim() == "show")
    {
        ShowItems(transactions, "all");
    }

    Transaction selectedItem = (Transaction)(from t in transactions where t.Title.ToLower() == input.ToLower() select t);

    if (action == "edit")
    {
        Console.WriteLine($"Are you sure you would like to edit {selectedItem.Title}?");
        Console.WriteLine("Enter 'Y' or 'YES' to confirm");
        string confirm = Console.ReadLine();
        if (confirm == "y".ToLower().Trim() || confirm == "yes".ToLower().Trim())
        {                          
            Console.WriteLine("Enter the new title for transaction:");
            string title = Console.ReadLine();

            // Update the selected item's title
            selectedItem.Title = title;

            Console.WriteLine("Enter the new amount of the transaction (must be positve number):");
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
                Console.WriteLine("The amount is to big, must be of type Integer.");
            }

            // Catch remaining errors and provide correct usage instuctions
            catch (Exception ex)
            {
                Console.WriteLine("The amount must be a positive number of the type Integer");
            }
        }
    }
    else if (action == "remove")
    {
        Console.WriteLine($"Are you sure you would like to remove {selectedItem.Title}?");
        Console.WriteLine("Enter 'Y' or 'YES' to confirm");
        string confirm = Console.ReadLine();
        if (confirm == "y".ToLower().Trim() || confirm == "yes".ToLower().Trim())
        {
            transactions.Remove(selectedItem);
        }
    }

    foreach (Transaction t in transactions)
    {
        if (input.ToLower() == t.Title.ToLower())
        {
            if (action == "edit")
            {
                Console.WriteLine($"Are you sure you would like to edit {t.Title}?");
                Console.WriteLine("Enter 'Y' or 'YES' to confirm");
                string confirm = Console.ReadLine();
                if (confirm == "y".ToLower().Trim() || confirm == "yes".ToLower().Trim())
                {
                    
                }

                
            }
            else if (action == "remove")
            {
                Console.WriteLine($"Are you sure you would like to remove {t.Title}?");
                Console.WriteLine("Enter 'Y' or 'YES' to confirm");
                string confirm = Console.ReadLine();
                if (confirm == "y".ToLower().Trim() || confirm == "yes".ToLower().Trim())
                {
                    transactions.Remove(t);
                }                
            }
        }
    }
}

