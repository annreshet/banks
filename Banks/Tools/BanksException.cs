namespace Banks.Tools;

public class BanksException : Exception
{
    public BanksException(string message)
        : base(message)
    {
    }
}