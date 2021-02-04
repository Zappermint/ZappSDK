namespace Zappermint
{
    public class Account
    {
        public string wallet { get; }
        public string name { get; }
        public Account(LoginData data)
        {
            wallet = data.wallet;
            name = data.name;
        }
    }
}