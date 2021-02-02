namespace Zappermint
{
    public class Account
    {
        public string wallet { get; }
        public string name { get; }
        public Account(LoginData data)
        {
            this.wallet = data.wallet;
            this.name = data.name;
        }
    }
}