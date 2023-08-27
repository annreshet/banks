using Banks.Tools;

namespace Banks.Models;

public class DepositInterest
{
    private const double MinimalDepositBalance = 0;
    private const double MinimalInterest = 0;
    private const double MaximalInterest = 100;
    public DepositInterest(double interest, double minimalBalance, double maximalBalance)
    {
        if (interest is < MinimalInterest or > MaximalInterest)
        {
            throw new BanksException("Interest rate should be between 0 and 100.");
        }

        if (minimalBalance < MinimalDepositBalance)
        {
            throw new BanksException("Minimal balance for deposit interest rate cannot be negative");
        }

        if (maximalBalance < MinimalDepositBalance)
        {
            throw new BanksException("Maximal balance for deposit interest rate cannot be negative");
        }

        if (maximalBalance < minimalBalance)
        {
            throw new BanksException("Minimal balance for deposit account terms should be greater than maximal balance (duh)");
        }

        Interest = interest;
        MinimalBalance = minimalBalance;
        MaximalBalance = maximalBalance;
    }

    public double Interest { get; }
    public double MinimalBalance { get; }
    public double MaximalBalance { get; }
}