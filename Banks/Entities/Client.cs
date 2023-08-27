using Banks.AccountFactory;
using Banks.Models;
using Banks.Observer;
using Banks.Tools;

namespace Banks.Entities;

public class Client : ISubscriber
{
    private const long MinimalPassportNumber = 1000000000;
    private const long MaximalPassportNumber = 9999999999;
    private List<IAccount> _accounts;
    public Client(string name, string surname, string? address, long? passport)
    {
        _accounts = new List<IAccount>();
        Name = name;
        Surname = surname;
        Address = address;
        Passport = passport;
        BankName = string.Empty;
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string? Address { get; private set; }
    public long? Passport { get; private set; }
    public bool Approved => Address != null && Passport != null;
    public IReadOnlyCollection<IAccount> Accounts => _accounts;
    public string BankName { get; private set; }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BanksException("Client name cannot be empty");
        }

        Name = name;
    }

    public void SetSurname(string surname)
    {
        if (string.IsNullOrWhiteSpace(surname))
        {
            throw new BanksException("Client name cannot be empty");
        }

        Surname = surname;
    }

    public void SetAddress(string address)
    {
        Address = address;
    }

    public void SetPassport(long passport)
    {
        if (passport is < MinimalPassportNumber or > MaximalPassportNumber)
        {
            throw new BanksException("Invalid passport number");
        }

        Passport = passport;
    }

    public void SetBankName(Bank bank)
    {
        BankName = bank.Name;
    }

    public void AddAccount(IAccount account)
    {
        _accounts.Add(account);
    }

    public void Update(IBank bank, AccountType type)
    {
        IAccount? account = Accounts.FirstOrDefault(account => account.GetAccountType() == type);
        if (account is not null)
        {
            // client gets a message
        }
    }
}