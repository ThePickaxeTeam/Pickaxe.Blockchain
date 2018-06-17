using System;
using System.Collections.Generic;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class Block
    {
        public Block()
        {
            Transactions = new List<Transaction>();
        }

        public int Index { get; set; }

        public IList<Transaction> Transactions { get; set; }

        public int Difficulty { get; set; }

        public byte[] PreviousBlockHash { get; set; }

        public string MinedBy { get; set; }

        public byte[] DataHash { get; set; }

        public ulong Nonce { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public byte[] Hash { get; set; }
    }
}
