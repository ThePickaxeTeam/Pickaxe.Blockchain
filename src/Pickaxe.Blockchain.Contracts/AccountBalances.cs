namespace Pickaxe.Blockchain.Contracts
{
    public class AccountBalances
    {
        public long SafeBalance { get; set; }

        public long ConfirmedBalance { get; set; }

        public long PendingBalance { get; set; }
    }
}
