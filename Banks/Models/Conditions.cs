using Banks.Tools;

namespace Banks.Models;

public class Conditions
{
    private const double MinimalInterest = 0;
    private const double MaximalInterest = 100;
    private const double MinimalLimit = 0;
    private const double MinimalDepositTime = 1;
    private List<DepositInterest> _depositInterests;
    public Conditions(
        double debitInterest,
        double creditLimit,
        double creditInterest,
        List<DepositInterest> depositInterests,
        int depositTime)
    {
        if (debitInterest is < MinimalInterest or > MaximalInterest)
        {
            throw new BanksException("Debit interest should be between 0 and 100.");
        }

        if (creditInterest is < MinimalInterest or > MaximalInterest)
        {
            throw new BanksException("Credit interest should be between 0 and 100.");
        }

        if (creditLimit < MinimalLimit)
        {
            throw new BanksException("Credit limit should be greater than zero.");
        }

        if (depositTime < MinimalDepositTime)
        {
            throw new BanksException("Deposit time should be at least 1");
        }

        DebitInterest = debitInterest;
        CreditLimit = creditLimit;
        CreditInterest = creditInterest;
        _depositInterests = depositInterests;
        DepositTime = depositTime;
    }

    public double DebitInterest { get; private set; }
    public double CreditLimit { get; private set; }
    public double CreditInterest { get; private set; }

    public IReadOnlyCollection<DepositInterest> DepositInterests
    {
        get => _depositInterests;
        private set => _depositInterests = (List<DepositInterest>)value;
    }

    public int DepositTime { get; private set; }

    public void SetDebitInterest(double debitInterest)
    {
        if (debitInterest is < MinimalInterest or > MaximalInterest)
        {
            throw new BanksException("Debit interest should be between 0 and 100.");
        }

        DebitInterest = debitInterest;
    }

    public void SetCreditLimit(double creditLimit)
    {
        if (creditLimit < MinimalLimit)
        {
            throw new BanksException("Credit limit should be greater than zero.");
        }

        CreditLimit = creditLimit;
    }

    public void SetCreditInterest(double creditInterest)
    {
        if (creditInterest is < MinimalInterest or > MaximalInterest)
        {
            throw new BanksException("Credit interest should be between 0 and 100.");
        }

        CreditInterest = creditInterest;
    }

    public void SetDepositTime(int depositTime)
    {
        if (depositTime < MinimalDepositTime)
        {
            throw new BanksException("Deposit time should be at least 1");
        }

        DepositTime = depositTime;
    }

    public void SetDepositInterests(IReadOnlyCollection<DepositInterest> depositInterests)
    {
        DepositInterests = depositInterests ?? throw new NullReferenceException(nameof(depositInterests));
    }
}