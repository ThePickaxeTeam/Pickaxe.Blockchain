namespace Pickaxe.Blockchain.Contracts
{
    public class NewBlockNotification
    {
        public int BlocksCount { get; set; }

        public long CumulativeDifficulty { get; set; }

        public string NodeUrl { get; set; }
    }
}
