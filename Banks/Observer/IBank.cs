using Banks.Models;

namespace Banks.Observer;

public interface IBank
{
    void Attach(ISubscriber subscriber);
    void Detach(ISubscriber subscriber);
    void Notify(AccountType type);
}