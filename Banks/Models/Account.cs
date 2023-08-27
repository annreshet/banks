using Banks.AccountFactory;
using Banks.Entities;
using Banks.Tools;

namespace Banks.Models;

public class Account : IAccount
{
    private const int MinimalId = 0;
    private List<Transaction> _transactions;

    protected Account(Client client, int id, AccountType type)
    {
        if (id < MinimalId)
        {
            throw new BanksException("ID cannot be negative");
        }

        Client = client ?? throw new NullReferenceException(nameof(client));
        Balance = 0;
        Id = id;
        Type = type;
        _transactions = new List<Transaction>();
    }

    public Client Client { get; }
    public double Balance { get; private set; }
    public int Id { get; }
    public IReadOnlyCollection<Transaction> Transactions => _transactions;

    public AccountType Type { get; }

    public int GetId()
    {
        return Id;
    }

    public AccountType GetAccountType()
    {
        return Type;
    }

    public virtual void ApplyConditions()
    {
    }

    public virtual void ApplyEverydayConditions()
    {
    }

    public virtual void UpdateConditions(Conditions conditions)
    {
    }

    public void SetBalance(double newBalance)
    {
        Balance = newBalance;
    }

    public virtual void Withdraw(double amount)
    {
    }

    public virtual void Add(double amount)
    {
    }

    public void Transfer(int recipientId, string bankName, double amount)
    {
        if (!Client.Approved)
        {
            throw new BanksException("Client is not approved, can't transfer money.");
        }

        var centralBank = CentralBank.GetCentralBank();
        centralBank.Transfer(Client.BankName, bankName, Id, recipientId, amount);
    }

    public void CancelLastTransaction()
    {
        if (!Client.Approved)
        {
            throw new BanksException("Client is not approved, cannot cancel transaction");
        }

        Transaction? transaction = Transactions.LastOrDefault();
        if (transaction is null)
        {
            throw new BanksException("There are no transactions to cancel");
        }

        switch (transaction.Type)
        {
            case TransactionType.Withdraw:
                Add(transaction.Amount);
                break;
            case TransactionType.Add:
                Withdraw(transaction.Amount);
                break;
            default:
                throw new BanksException("Transaction with this type doesn't exist");
        }

        RemoveTransaction(transaction);
    }

    public void AddTransaction(Transaction transaction)
    {
        _transactions.Add(transaction);
    }

    public double GetBalance()
    {
        return Balance;
    }

    public void RemoveTransaction(Transaction transaction)
    {
        _transactions.Remove(transaction);
    }
}