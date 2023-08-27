using Banks.Entities;

namespace Banks.ClientBuilder;

public class Director
{
    private IBuilder _builder = new ClientBuilder();

    public void BuildUnapprovedClient(string name, string surname)
    {
        _builder.SetName(name);
        _builder.SetSurname(surname);
    }

    public void BuildUnapprovedClient(string name, string surname, string address)
    {
        _builder.SetName(name);
        _builder.SetSurname(surname);
        _builder.SetAddress(address);
    }

    public void BuildUnapprovedClient(string name, string surname, int passport)
    {
        _builder.SetName(name);
        _builder.SetSurname(surname);
        _builder.SetPassport(passport);
    }

    public void BuildApprovedClient(string name, string surname, string address, int passport)
    {
        _builder.SetName(name);
        _builder.SetSurname(surname);
        _builder.SetAddress(address);
        _builder.SetPassport(passport);
    }

    public Client GetClient()
    {
        return _builder.GetClient();
    }
}