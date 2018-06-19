using System.Collections.Generic;

namespace Pickaxe.Blockchain.Contracts
{
    public class Block
    {
        public int Index { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public int Difficulty { get; set; }

        public string PreviousBlockHash { get; set; }

        public string MinedBy { get; set; }

        public string BlockDataHash { get; set; }

        public ulong Nonce { get; set; }

        public string DateCreated { get; set; }

        public string BlockHash { get; set; }
    }
}
