using Banks.Models;

namespace Banks.Observer;

public interface ISubscriber
{
    void Update(IBank bank, AccountType type);
}