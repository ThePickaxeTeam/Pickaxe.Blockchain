namespace Pickaxe.Blockchain.Contracts
{
    public class MiningJob
    {
        /// <summary>
        /// The index of the new block, the one which needs to be mined.
        /// </summary>
        public int BlockIndex { get; set; }

        public int Difficulty { get; set; }

        public long ExpectedReward { get; set; }

        public string RewardAddress { get; set; }

        public int TransactionsIncluded { get; set; }

        public string BlockDataHash { get; set; }
    }
}
