using Banks.AccountFactory;
using Banks.Models;
using Banks.Tools;

namespace Banks.Entities;

public class CreditAccount : Account
{
    private const double MaximumBalanceToApplyConditions = 0;
    private double _creditInterestBalance = 0;
    public CreditAccount(Client client, int id, AccountType type, double limit, double interest)
        : base(client, id, type)
    {
        Limit = limit;
        Interest = interest;
    }

    public double Limit { get; private set; }
    public double Interest { get; private set; }

    public override void ApplyEverydayConditions()
    {
        if ((Balance + _creditInterestBalance) < -Limit)
        {
            throw new BanksException("Credit card limit was exceeded");
        }

        if (Balance + _creditInterestBalance >= MaximumBalanceToApplyConditions) return;
        _creditInterestBalance += Balance * (Interest / 100);
    }

    public override void ApplyConditions()
    {
        SetBalance(Balance + _creditInterestBalance);
        _creditInterestBalance = 0;
    }

    public override void UpdateConditions(Conditions conditions)
    {
        Limit = conditions.CreditLimit;
        Interest = conditions.CreditInterest;
    }

    public override void Withdraw(double amount)
    {
        if (!Client.Approved)
        {
            throw new BanksException("Client is not approved, can't withdraw.");
        }

        double newBalance = Balance - amount;
        if (newBalance < -Limit)
        {
            throw new BanksException("Withdrawing this amount exceeds credit account limit");
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