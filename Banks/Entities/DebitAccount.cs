using Banks.AccountFactory;
using Banks.Models;
using Banks.Tools;

namespace Banks.Entities;

public class DebitAccount : Account
{
    private const double MinimalDebitBalance = 0;
    private double _debitInterestBalance = 0;
    public DebitAccount(Client client, int id, AccountType type, double interest)
        : base(client, id, type)
    {
        Interest = interest;
    }

    public double Interest { get; private set; }

    public override void ApplyEverydayConditions()
    {
        _debitInterestBalance += Balance * (Interest / 100);
    }

    public override void ApplyConditions()
    {
        SetBalance(Balance + _debitInterestBalance);
        _debitInterestBalance = 0;
    }

    public override void UpdateConditions(Conditions conditions)
    {
        Interest = conditions.DebitInterest;
    }

    public override void Withdraw(double amount)
    {
        if (!Client.Approved)
        {
            throw new BanksException("Client is not approved, can't withdraw.");
        }

        double newBalance = Balance - amount;
        if (newBalance < MinimalDebitBalance)
        {
            throw new BanksException("Not enough money in debit account to withdraw.");
        }

        SetBalance(newBalance);
        AddTransaction(new Transaction(TransactionType.Withdraw, amount));
    }

    public override void Add(double amount)
    {
        SetBalance(Balance + amount);
        AddTransaction(new Transaction(TransactionType.Add, amount));
    }
}