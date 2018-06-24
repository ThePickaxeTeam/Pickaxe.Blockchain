namespace Pickaxe.Blockchain.Contracts
{
    public class NodeInfo
    {
        public string About { get; set; }

        public string NodeId { get; set; }

        public string ChainId { get; set; }

        public string NodeUrl { get; set; }

        public int Peers { get; set; }

        public int CurrentDifficulty { get; set; }

        public int BlocksCount { get; set; }

        public long CumulativeDifficulty { get; set; }

        public int ConfirmedTransactions { get; set; }

        public int PendingTransactions { get; set; }
    }
}
