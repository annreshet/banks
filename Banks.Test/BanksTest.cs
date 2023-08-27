using Banks.AccountFactory;
using Banks.Entities;
using Banks.Models;
using Xunit;

namespace Banks.Test;

public class BanksTest
{
    [Fact]
    public void TestId()
    {
        var centralBank = CentralBank.GetCentralBank();
        var depositInterests = new List<DepositInterest>() { new DepositInterest(2, 0, 10000) };
        var conditions = new Conditions(1, 20000, 4, depositInterests, 3);
        Bank bank = centralBank.CreateBank("Sber", conditions);
        var client = new Client("Anna", "Reshetnikova", "home", 1234567890);
        IAccount account1 = bank.CreateAccount(client, AccountType.Credit);
        IAccount account2 = bank.CreateAccount(client, AccountType.Debit);
        Assert.NotEqual(account1.GetId(), account2.GetId());
    }
}