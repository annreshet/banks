using Banks.AccountFactory;

namespace Banks.Models;

public class Transaction
{
    public Transaction(TransactionType type, double amount)
    {
        Type = type;
        Amount = amount;
    }

    public TransactionType Type { get; }
    public double Amount { get; }
}