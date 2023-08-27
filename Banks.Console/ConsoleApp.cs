using Banks.AccountFactory;
using Banks.Entities;
using Banks.Models;

namespace Banks.Console;

public class ConsoleApp
{
    private readonly CentralBank _centralBank = CentralBank.GetCentralBank();
    private readonly People _people = People.GetPeople();
    private Person _person = new ();

    public void Start()
    {
        System.Console.WriteLine(
            "What is your role?\n" +
            "Enter an option number\n" +
            "1. Central bank\n" +
            "2. Regular person\n" +
            "3. Exit\n");
        string? role = System.Console.ReadLine();
        switch (role)
        {
            case "1":
                CentralBankConsole();
                break;
            case "2":
                UserConsole();
                break;
            case "3":
                return;
            default:
                System.Console.WriteLine("Invalid input.");
                Start();
                break;
        }
    }

    public void CentralBankConsole()
    {
        System.Console.WriteLine(
            "List of central bank options, enter a number:\n" +
            "1. Create new bank\n" +
            "2. Apply conditions\n" +
            "3. Exit to main\n");
        string? option = System.Console.ReadLine();
        switch (option)
        {
            case "1":
                CreateBank();
                break;
            case "2":
                ApplyConditions();
                break;
            case "3":
                Start();
                break;
        }
    }

    public void CreateBank()
    {
        string? name = string.Empty;
        while (string.IsNullOrWhiteSpace(name))
        {
            System.Console.WriteLine("Enter the bank's name:");
            name = System.Console.ReadLine();
        }

        System.Console.WriteLine("Enter bank's debit interest");
        double debitInterest = Convert.ToDouble(System.Console.ReadLine());
        System.Console.WriteLine("Enter bank's credit limit");
        double creditLimit = Convert.ToDouble(System.Console.ReadLine());
        System.Console.WriteLine("Enter bank's credit interest");
        double creditInterest = Convert.ToDouble(System.Console.ReadLine());
        List<DepositInterest> depositInterests = DepositInterests();
        System.Console.WriteLine("Enter bank's deposit time in months");
        int depositTime = Convert.ToInt32(System.Console.ReadLine());
        var conditions = new Conditions(debitInterest, creditLimit, creditInterest, depositInterests, depositTime);
        _centralBank.CreateBank(name, conditions);
        CentralBankConsole();
    }

    public List<DepositInterest> DepositInterests()
    {
        System.Console.WriteLine("Set deposit interests");
        var depositInterests = new List<DepositInterest>();
        string? response = "y";
        while (response == "y")
        {
            System.Console.WriteLine("Enter deposit interest rate");
            double interest = Convert.ToDouble(System.Console.ReadLine());
            System.Console.WriteLine("Enter minimal deposit balance for this rate");
            double minimalBalance = Convert.ToDouble(System.Console.ReadLine());
            System.Console.WriteLine("Enter maximal deposit balance for this rate");
            double maximalBalance = Convert.ToDouble(System.Console.ReadLine());
            depositInterests.Add(new DepositInterest(interest, minimalBalance, maximalBalance));
            System.Console.WriteLine("Do you want to set another deposit interest? y/n");
            response = System.Console.ReadLine();
        }

        return depositInterests;
    }

    public void ApplyConditions()
    {
        System.Console.WriteLine("Enter the amount of days");
        int days = Convert.ToInt32(System.Console.ReadLine());
        var timeSpan = TimeSpan.FromDays(days);
        _centralBank.ApplyConditions(timeSpan);
        CentralBankConsole();
    }

    public void UserConsole()
    {
        System.Console.WriteLine("Are you registered? y/n");
        string? response = System.Console.ReadLine();
        switch (response)
        {
            case "y":
                string? name = string.Empty;
                while (string.IsNullOrWhiteSpace(name))
                {
                    System.Console.WriteLine("Enter your first name:");
                    name = System.Console.ReadLine();
                }

                string? surname = string.Empty;
                while (string.IsNullOrWhiteSpace(surname))
                {
                    System.Console.WriteLine("Enter your last name:");
                    surname = System.Console.ReadLine();
                }

                _person = _people.GetPerson(name, surname);
                UserOptions();
                break;
            case "n":
                UserRegister();
                break;
        }
    }

    public void UserRegister()
    {
        const long minimalPassportNumber = 1000000000;
        const long maximalPassportNumber = 9999999999;
        string? name = string.Empty;
        while (string.IsNullOrWhiteSpace(name))
        {
            System.Console.WriteLine("Enter your first name:");
            name = System.Console.ReadLine();
        }

        string? surname = string.Empty;
        while (string.IsNullOrWhiteSpace(surname))
        {
            System.Console.WriteLine("Enter your last name:");
            surname = System.Console.ReadLine();
        }

        string? address = null;
        long? passport = null;

        System.Console.WriteLine("Do you want to enter your address? y/n");
        string? response = System.Console.ReadLine();
        if (response == "y")
        {
            while (string.IsNullOrWhiteSpace(address))
            {
                System.Console.WriteLine("Enter the address:");
                address = System.Console.ReadLine();
            }
        }

        System.Console.WriteLine("Do you want to enter your passport information? y/n");
        response = System.Console.ReadLine();
        if (response == "y")
        {
            while (passport is null or < minimalPassportNumber or > maximalPassportNumber)
            {
                System.Console.WriteLine("Enter the passport information:");
                passport = Convert.ToInt64(System.Console.ReadLine());
            }
        }

        _person = new Person(name, surname, address, passport);
        _people.AddPerson(_person);

        System.Console.WriteLine("Finished with your information.");
        UserOptions();
    }

    public void UserOptions()
    {
        System.Console.WriteLine(
            "List of options. Enter a number:\n" +
            "1. Change my personal information\n" +
            "2. Open a new bank account\n" +
            "3. Make a transaction\n" +
            "4. Show my bank accounts\n" +
            "5. Rewind time\n" +
            "6. Exit to main\n");
        string? option = System.Console.ReadLine();
        switch (option)
        {
            case "1":
                ChangePersonalInformation();
                break;
            case "2":
                OpenBankAccount();
                break;
            case "3":
                MakeTransaction();
                break;
            case "4":
                ShowAccounts();
                break;
            case "5":
                Start();
                break;
        }
    }

    public void ChangePersonalInformation()
    {
        System.Console.WriteLine(
            "Choose what you want to change\n" +
            "Enter a number:\n" +
            "1. First name\n" +
            "2. Last name\n" +
            "3. Address\n" +
            "4. Passport information\n" +
            "5. Go back to options\n");
        string? response = System.Console.ReadLine();
        switch (response)
        {
            case "1":
                string? name = string.Empty;
                while (string.IsNullOrWhiteSpace(name))
                {
                    System.Console.WriteLine("Enter new first name");
                    name = System.Console.ReadLine();
                }

                _person.SetName(name);
                foreach (Client bankClient in _person.BankClients)
                {
                    bankClient.SetName(name);
                }

                break;
            case "2":
                string? surname = string.Empty;
                while (string.IsNullOrWhiteSpace(surname))
                {
                    System.Console.WriteLine("Enter new last name");
                    surname = System.Console.ReadLine();
                }

                _person.SetSurname(surname);
                foreach (Client bankClient in _person.BankClients)
                {
                    bankClient.SetSurname(surname);
                }

                break;
            case "3":
                string? address = string.Empty;
                while (string.IsNullOrWhiteSpace(address))
                {
                    System.Console.WriteLine("Enter new address");
                    address = System.Console.ReadLine();
                }

                _person.SetAddress(address);
                foreach (Client bankClient in _person.BankClients)
                {
                    if (bankClient.Address is not null)
                    {
                        bankClient.SetAddress(address);
                    }
                }

                break;
            case "4":
                const int minimalPassportNumber = 1000000000;
                long? passport = 0;
                while (passport is < minimalPassportNumber or null)
                {
                    System.Console.WriteLine("Enter new passport number");
                    passport = Convert.ToInt64(System.Console.ReadLine());
                }

                _person.SetPassport((long)passport);
                foreach (Client bankClient in _person.BankClients)
                {
                    if (bankClient.Passport is not null)
                    {
                        bankClient.SetPassport((long)passport);
                    }
                }

                break;
            case "5":
                UserOptions();
                break;
        }

        UserOptions();
    }

    public void OpenBankAccount()
    {
        System.Console.WriteLine("Enter the name of the bank.");
        string? bankName = System.Console.ReadLine();
        Bank bank = _centralBank.GetBank(bankName);

        Client client = _person.BankClients.SingleOrDefault(client => client.BankName == bankName) ?? BecomeBankClient();
        System.Console.WriteLine(
            "Choose the type of account to open.\n" +
            "Enter a number.\n" +
            "1. Debit account\n" +
            "2. Credit account\n" +
            "3. Deposit account\n");
        string? response = System.Console.ReadLine();
        string? answer;
        switch (response)
        {
            case "1":
                bank.CreateAccount(client, AccountType.Debit);
                System.Console.WriteLine($"You've opened a debit account in {bankName}.");
                System.Console.WriteLine("Do you want to subscribe to notifications from bank? y/n");
                answer = System.Console.ReadLine();
                if (answer == "y")
                {
                    client.Update(bank, AccountType.Debit);
                }

                break;
            case "2":
                bank.CreateAccount(client, AccountType.Credit);
                System.Console.WriteLine($"You've opened a credit account in {bankName}.");
                System.Console.WriteLine("Do you want to subscribe to notifications from bank? y/n");
                answer = System.Console.ReadLine();
                if (answer == "y")
                {
                    client.Update(bank, AccountType.Credit);
                }

                break;
            case "3":
                bank.CreateAccount(client, AccountType.Deposit);
                System.Console.WriteLine($"You've opened a deposit account in {bankName}.");
                System.Console.WriteLine("Do you want to subscribe to notifications from bank? y/n");
                answer = System.Console.ReadLine();
                if (answer == "y")
                {
                    client.Update(bank, AccountType.Debit);
                }

                break;
        }

        UserOptions();
    }

    public Client BecomeBankClient()
    {
        var builder = new ClientBuilder.ClientBuilder();
        builder.SetName(_person.Name);
        builder.SetSurname(_person.Surname);
        if (_person.Address is not null)
        {
            System.Console.WriteLine("Do you want to give the bank information about your address? y/n");
            string? response = System.Console.ReadLine();
            if (response == "y")
            {
                builder.SetAddress(_person.Address);
            }
        }
        else
        {
            System.Console.WriteLine("Do you want to set an address information and give it to the bank? y/n");
            string? response = System.Console.ReadLine();
            if (response == "y")
            {
                string? address = string.Empty;
                while (string.IsNullOrWhiteSpace(address))
                {
                    System.Console.WriteLine("Enter address information");
                    address = System.Console.ReadLine();
                }

                _person.SetAddress(address);
                builder.SetAddress(address);
            }
        }

        if (_person.Passport is not null)
        {
            System.Console.WriteLine("Do you want to give the bank information about your passport? y/n");
            string? response = System.Console.ReadLine();
            if (response == "y")
            {
                builder.SetPassport((long)_person.Passport);
            }
        }
        else
        {
            System.Console.WriteLine("Do you want to set passport information and give it to the bank? y/n");
            string? response = System.Console.ReadLine();
            if (response == "y")
            {
                const int minimalPassportNumber = 1000000000;
                long? passport = null;
                while (passport is < minimalPassportNumber or null)
                {
                    System.Console.WriteLine("Enter passport information");
                    passport = Convert.ToInt64(System.Console.ReadLine());
                }

                _person.SetPassport((long)passport);
                builder.SetPassport((long)passport);
            }
        }

        Client client = builder.GetClient();
        _person.AddClient(client);
        return client;
    }

    public void MakeTransaction()
    {
        System.Console.WriteLine("Enter the name of the bank");
        string? bankName = System.Console.ReadLine();
        Bank bank = _centralBank.GetBank(bankName);
        IReadOnlyCollection<IAccount> accounts = new List<IAccount>().AsReadOnly();
        Client? client = _person.BankClients.SingleOrDefault(client => client.BankName == bankName);
        if (client == null)
        {
            System.Console.WriteLine("You don't have accounts in this bank");
            UserOptions();
        }
        else
        {
            accounts = client.Accounts;
        }

        foreach (IAccount account in accounts)
        {
            System.Console.WriteLine($"Account ID: {account.GetId()}, Account type: {account.GetAccountType().ToString()}, Balance: {account.GetBalance():N}");
        }

        IAccount? bankAccount = null;
        while (bankAccount is null)
        {
            System.Console.WriteLine("Enter the ID of an account to make a transaction");
            int accountId = Convert.ToInt32(System.Console.ReadLine());
            bankAccount = accounts.FirstOrDefault(account => account.GetId() == accountId);
            if (bankAccount is null)
            {
                System.Console.WriteLine("Invalid id");
            }
        }

        System.Console.WriteLine("Enter an amount for transaction");
        double amount = Convert.ToDouble(System.Console.ReadLine());
        System.Console.WriteLine(
            "Choose the type of transaction. Enter a number\n" +
            "1. Add\n" +
            "2. Withdraw\n" +
            "3. Transfer\n");
        string? response = System.Console.ReadLine();
        switch (response)
        {
            case "1":
                bankAccount.Add(amount);
                System.Console.WriteLine($"New balance of {bankAccount.GetAccountType().ToString()} account (ID {bankAccount.GetId()}) is {bankAccount.GetBalance()}");
                break;
            case "2":
                bankAccount.Withdraw(amount);
                System.Console.WriteLine($"New balance of {bankAccount.GetAccountType().ToString()} account (ID {bankAccount.GetId()}) is {bankAccount.GetBalance()}");
                break;
            case "3":
                Transfer(bankAccount, amount);
                System.Console.WriteLine($"New balance of {bankAccount.GetAccountType().ToString()} account (ID {bankAccount.GetId()}) is {bankAccount.GetBalance()}");
                break;
        }

        UserOptions();
    }

    public void Transfer(IAccount senderAccount, double amount)
    {
        string? recipientBankName = null;
        while (string.IsNullOrWhiteSpace(recipientBankName))
        {
            System.Console.WriteLine("Enter recipient bank name");
            recipientBankName = System.Console.ReadLine();
        }

        int recipientId = 0;
        while (recipientId <= 0)
        {
            System.Console.WriteLine("Enter recipient account id");
            recipientId = Convert.ToInt32(System.Console.ReadLine());
        }

        senderAccount.Transfer(recipientId, recipientBankName, amount);
    }

    public void ShowAccounts()
    {
        foreach (Client client in _person.BankClients)
        {
            System.Console.WriteLine($"Your accounts in {client.BankName}:");
            foreach (IAccount account in client.Accounts)
            {
                System.Console.WriteLine($"Account type: {account.GetAccountType().ToString()}, Balance: {account.GetBalance():N}");
            }
        }

        UserOptions();
    }
}