namespace Pickaxe.Blockchain.Domain.Serialization
{
    internal class BlockData
    {
        public int Index { get; set; }

        public TransactionData[] Transactions { get; set; }

        public int Difficulty { get; set; }

        public string PreviousBlockHash { get; set; }

        public string MinedBy { get; set; }
    }
}
