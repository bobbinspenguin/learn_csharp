using System;

namespace ExceptionHandlingDemo
{
    // Custom exception classes
    public class InsufficientFundsException : Exception
    {
        public decimal RequestedAmount { get; }
        public decimal AvailableBalance { get; }

        public InsufficientFundsException(decimal requestedAmount, decimal availableBalance)
            : base($"Insufficient funds. Requested: {requestedAmount:C}, Available: {availableBalance:C}")
        {
            RequestedAmount = requestedAmount;
            AvailableBalance = availableBalance;
        }
    }

    public class InvalidAccountException : Exception
    {
        public InvalidAccountException(string accountNumber)
            : base($"Account number '{accountNumber}' is invalid or does not exist.")
        {
        }
    }

    public class NegativeAmountException : Exception
    {
        public NegativeAmountException()
            : base("Amount cannot be negative or zero.")
        {
        }
    }

    // Simple Banking Application
    public class BankAccount
    {
        public string AccountNumber { get; private set; }
        public string AccountHolder { get; private set; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountNumber, string accountHolder, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty.");
            
            if (string.IsNullOrWhiteSpace(accountHolder))
                throw new ArgumentException("Account holder name cannot be empty.");
            
            if (initialBalance < 0)
                throw new NegativeAmountException();

            AccountNumber = accountNumber;
            AccountHolder = accountHolder;
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new NegativeAmountException();

            Balance += amount;
            Console.WriteLine($"Deposited {amount:C}. New balance: {Balance:C}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new NegativeAmountException();

            if (amount > Balance)
                throw new InsufficientFundsException(amount, Balance);

            Balance -= amount;
            Console.WriteLine($"Withdrew {amount:C}. New balance: {Balance:C}");
        }

        public void DisplayAccountInfo()
        {
            Console.WriteLine($"Account: {AccountNumber} | Holder: {AccountHolder} | Balance: {Balance:C}");
        }
    }

    public class BankingSystem
    {
        private static Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();

        public static void CreateAccount(string accountNumber, string accountHolder, decimal initialBalance = 0)
        {
            try
            {
                if (accounts.ContainsKey(accountNumber))
                    throw new ArgumentException($"Account {accountNumber} already exists.");

                var account = new BankAccount(accountNumber, accountHolder, initialBalance);
                accounts[accountNumber] = account;
                Console.WriteLine($"Account {accountNumber} created successfully for {accountHolder}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
            }
        }

        public static BankAccount GetAccount(string accountNumber)
        {
            if (!accounts.ContainsKey(accountNumber))
                throw new InvalidAccountException(accountNumber);

            return accounts[accountNumber];
        }

        public static void DisplayAllAccounts()
        {
            Console.WriteLine("\n--- All Accounts ---");
            if (accounts.Count == 0)
            {
                Console.WriteLine("No accounts found.");
                return;
            }

            foreach (var account in accounts.Values)
            {
                account.DisplayAccountInfo();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Exception Handling Demo - Banking System ===\n");

            // Initialize some test accounts
            InitializeTestData();

            bool continueProgram = true;
            while (continueProgram)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit Money");
                Console.WriteLine("3. Withdraw Money");
                Console.WriteLine("4. View Account");
                Console.WriteLine("5. View All Accounts");
                Console.WriteLine("6. Demonstrate Exception Scenarios");
                Console.WriteLine("7. Exit");
                Console.Write("\nEnter your choice (1-7): ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            CreateAccountInteractive();
                            break;
                        case "2":
                            DepositInteractive();
                            break;
                        case "3":
                            WithdrawInteractive();
                            break;
                        case "4":
                            ViewAccountInteractive();
                            break;
                        case "5":
                            BankingSystem.DisplayAllAccounts();
                            break;
                        case "6":
                            DemonstrateExceptionScenarios();
                            break;
                        case "7":
                            continueProgram = false;
                            Console.WriteLine("Thank you for using our banking system!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void InitializeTestData()
        {
            BankingSystem.CreateAccount("001", "John Doe", 1000);
            BankingSystem.CreateAccount("002", "Jane Smith", 500);
            BankingSystem.CreateAccount("003", "Bob Johnson", 2000);
        }

        static void CreateAccountInteractive()
        {
            try
            {
                Console.Write("Enter account number: ");
                string accountNumber = Console.ReadLine() ?? "";
                
                Console.Write("Enter account holder name: ");
                string accountHolder = Console.ReadLine() ?? "";
                
                Console.Write("Enter initial balance (or press Enter for 0): ");
                string balanceInput = Console.ReadLine() ?? "";
                
                decimal initialBalance = 0;
                if (!string.IsNullOrEmpty(balanceInput))
                {
                    if (!decimal.TryParse(balanceInput, out initialBalance))
                        throw new FormatException("Invalid balance format.");
                }

                BankingSystem.CreateAccount(accountNumber, accountHolder, initialBalance);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void DepositInteractive()
        {
            try
            {
                Console.Write("Enter account number: ");
                string accountNumber = Console.ReadLine() ?? "";
                
                var account = BankingSystem.GetAccount(accountNumber);
                
                Console.Write("Enter deposit amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    account.Deposit(amount);
                }
                else
                {
                    throw new FormatException("Invalid amount format.");
                }
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Account Error: {ex.Message}");
            }
            catch (NegativeAmountException ex)
            {
                Console.WriteLine($"Amount Error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Format Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void WithdrawInteractive()
        {
            try
            {
                Console.Write("Enter account number: ");
                string accountNumber = Console.ReadLine() ?? "";
                
                var account = BankingSystem.GetAccount(accountNumber);
                account.DisplayAccountInfo();
                
                Console.Write("Enter withdrawal amount: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    account.Withdraw(amount);
                }
                else
                {
                    throw new FormatException("Invalid amount format.");
                }
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Account Error: {ex.Message}");
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"Transaction Error: {ex.Message}");
                Console.WriteLine($"You tried to withdraw {ex.RequestedAmount:C} but only have {ex.AvailableBalance:C}");
            }
            catch (NegativeAmountException ex)
            {
                Console.WriteLine($"Amount Error: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Format Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void ViewAccountInteractive()
        {
            try
            {
                Console.Write("Enter account number: ");
                string accountNumber = Console.ReadLine() ?? "";
                
                var account = BankingSystem.GetAccount(accountNumber);
                account.DisplayAccountInfo();
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Account Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void DemonstrateExceptionScenarios()
        {
            Console.WriteLine("\n=== Exception Handling Scenarios ===");

            // Scenario 1: Try-catch-finally
            Console.WriteLine("\n1. Try-Catch-Finally Example:");
            TryCatchFinallyDemo();

            // Scenario 2: Multiple catch blocks
            Console.WriteLine("\n2. Multiple Catch Blocks Example:");
            MultipleCatchDemo();

            // Scenario 3: Custom exceptions
            Console.WriteLine("\n3. Custom Exception Example:");
            CustomExceptionDemo();
        }

        static void TryCatchFinallyDemo()
        {
            FileStream? fileStream = null;
            try
            {
                Console.WriteLine("Attempting to open a file...");
                // This will throw an exception since the file doesn't exist
                fileStream = new FileStream("nonexistent.txt", FileMode.Open);
                Console.WriteLine("File opened successfully.");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }
            finally
            {
                // This block always executes
                Console.WriteLine("Cleanup: Closing file stream if it was opened.");
                fileStream?.Close();
            }
        }

        static void MultipleCatchDemo()
        {
            try
            {
                Console.Write("Enter a number to divide 100 by: ");
                string input = Console.ReadLine() ?? "";
                
                int number = int.Parse(input);
                int result = 100 / number;
                Console.WriteLine($"100 / {number} = {result}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Error: Please enter a valid integer.");
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("Error: Cannot divide by zero.");
            }
            catch (OverflowException)
            {
                Console.WriteLine("Error: Number is too large.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void CustomExceptionDemo()
        {
            try
            {
                // This will trigger our custom exception
                var account = BankingSystem.GetAccount("001");
                account.Withdraw(5000); // More than the balance
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"Custom Exception Caught: {ex.Message}");
                Console.WriteLine($"Additional Info - Requested: {ex.RequestedAmount:C}, Available: {ex.AvailableBalance:C}");
            }
        }
    }
}
