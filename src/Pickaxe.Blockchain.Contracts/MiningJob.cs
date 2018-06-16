namespace Pickaxe.Blockchain.Contracts
{
    public class MiningJob
    {
        /// <summary>
        /// The index of the new block, the one which needs to be mined.
        /// </summary>
        public int BlockIndex { get; set; }

        public string TransactionsHash { get; set; }

        public string PreviousBlockHash { get; set; }

        public int Difficulty { get; set; }
    }
}
