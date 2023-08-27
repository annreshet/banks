using System.Collections.Immutable;
using Banks.AccountFactory;
using Banks.Models;
using Banks.Tools;

namespace Banks.Entities;

public class DepositAccount : Account
{
    private const double MinimalDepositBalance = 0;
    private double _depositInterestBalance = 0;
    public DepositAccount(Client client, int id, AccountType type, int depositTime, IReadOnlyCollection<DepositInterest> depositInterests)
        : base(client, id, type)
    {
        DateTime now = DateTime.Now;
        DueTime = now.AddMonths(depositTime);
        DepositInterests = depositInterests;
    }

    public DateTime DueTime { get; }
    public IReadOnlyCollection<DepositInterest> DepositInterests { get; private set; }

    public override void ApplyEverydayConditions()
    {
        if (DateTime.Now > DueTime) return;
        DepositInterest? depositInterest = DepositInterests.FirstOrDefault(depositInterest =>
            Balance >= depositInterest.MinimalBalance && Balance < depositInterest.MaximalBalance);
        if (depositInterest is null)
        {
            throw new BanksException("Deposit account doesn't have conditions for the balance");
        }

        _depositInterestBalance += Balance * (depositInterest.Interest / 100);
    }

    public override void ApplyConditions()
    {
        SetBalance(Balance + _depositInterestBalance);
        _depositInterestBalance = 0;
    }

    public override void UpdateConditions(Conditions conditions)
    {
        DepositInterests = conditions.DepositInterests;
    }

    public override void Withdraw(double amount)
    {
        if (!Client.Approved)
        {
            throw new BanksException("Client is not approved, can't withdraw.");
        }

        if (DateTime.Now < DueTime)
        {
            throw new BanksException("Can't withdraw money from deposit account until its due date.");
        }

        double newBalance = Balance - amount;
        if (newBalance < MinimalDepositBalance)
        {
            throw new BanksException("Not enough money in deposit account to withdraw.");
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