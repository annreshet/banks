using Banks.AccountFactory;
using Banks.ClientBuilder;
using Banks.Models;
using Banks.Observer;
using Banks.Tools;

namespace Banks.Entities;

public class Bank : IBank
{
    private List<Client> _clients = new ();
    private List<ISubscriber> _subscribers = new ();
    private int _id;
    public Bank(string name, Conditions conditions)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BanksException("Bank name cannot be empty");
        }

        Name = name;
        BankConditions = conditions ?? throw new NullReferenceException(nameof(conditions));
    }

    public string Name { get; }
    public Conditions BankConditions { get; }
    public IReadOnlyCollection<Client> Clients => _clients;

    public IAccount CreateAccount(Client client, AccountType type)
    {
        if (client.BankName == string.Empty)
        {
            client.SetBankName(this);
            _clients.Add(client);
        }

        switch (type)
        {
            case AccountType.Credit:
                AccountCreator creditCreator = new CreditAccountCreator();
                IAccount creditAccount = creditCreator.CreateAccount(this, client, ++_id, AccountType.Credit);
                client.AddAccount(creditAccount);

                return creditAccount;
            case AccountType.Debit:
                AccountCreator debitCreator = new DebitAccountCreator();
                IAccount debitAccount = debitCreator.CreateAccount(this, client, ++_id, AccountType.Debit);
                client.AddAccount(debitAccount);

                return debitAccount;
            case AccountType.Deposit:
                AccountCreator depositCreator = new DepositAccountCreator();
                IAccount depositAccount = depositCreator.CreateAccount(this, client, ++_id, AccountType.Deposit);
                client.AddAccount(depositAccount);

                return depositAccount;
            default:
                throw new BanksException("Choose a valid account type");
        }
    }

    public void UpdateDebitConditions(double newDebitInterest)
    {
        BankConditions.SetDebitInterest(newDebitInterest);

        UpdateAccountConditions(AccountType.Debit);
    }

    public void UpdateCreditConditions(double newCreditInterest, double newCreditLimit)
    {
        BankConditions.SetCreditInterest(newCreditInterest);
        BankConditions.SetCreditLimit(newCreditLimit);

        UpdateAccountConditions(AccountType.Credit);
    }

    public void UpdateDepositConditions(int newDepositTime, IReadOnlyCollection<DepositInterest> newDepositInterests)
    {
        BankConditions.SetDepositTime(newDepositTime);
        BankConditions.SetDepositInterests(newDepositInterests);

        UpdateAccountConditions(AccountType.Deposit);
    }

    public void UpdateBankConditions(Conditions conditions)
    {
        UpdateCreditConditions(conditions.CreditInterest, conditions.CreditLimit);
        UpdateDebitConditions(conditions.DebitInterest);
        UpdateDepositConditions(conditions.DepositTime, conditions.DepositInterests);
    }

    public void Attach(ISubscriber subscriber)
    {
        if (_subscribers.Contains(subscriber))
        {
            throw new BanksException("You are already subscribed to notifications from this bank");
        }

        _subscribers.Add(subscriber);
    }

    public void Detach(ISubscriber subscriber)
    {
        if (!_subscribers.Contains(subscriber))
        {
            throw new BanksException("You are not subscribed to notifications from this bank");
        }

        _subscribers.Remove(subscriber);
    }

    public void Notify(AccountType type)
    {
        foreach (ISubscriber subscriber in _subscribers)
        {
            subscriber.Update(this, type);
        }
    }

    private void UpdateAccountConditions(AccountType type)
    {
        IEnumerable<IAccount> accounts = _clients.SelectMany(client => client.Accounts.Where(account => account.GetAccountType() == type));
        foreach (IAccount account in accounts)
        {
            account.UpdateConditions(BankConditions);
        }

        Notify(type);
    }
}