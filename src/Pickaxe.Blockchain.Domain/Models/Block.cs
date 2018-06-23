using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Domain.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class Block
    {
        private byte[] _dataHash;

        public Block()
        {
            Transactions = new List<Transaction>();
        }

        public static Block CreateGenesisBlock()
        {
            Block genesisBlock = new Block
            {
                Index = 0,
                Transactions = new List<Transaction>
                {
                    Transaction.CreateGenesisTransaction(
                        "c3293572dbe6ebc60de4a20ed0e21446cae66b17")
                },
                Difficulty = 0,
                PreviousBlockHash = null,
                MinedBy = new string('0', 40),
                Nonce = 0,
                DateCreated = DateTime.UtcNow,
                MinerProvidedHash = new string('0', 64)
            };

            return genesisBlock;
        }

        public int Index { get; set; }

        public bool IsGenesis
        {
            get
            {
                return Index == 0;
            }
        }

        public IList<Transaction> Transactions { get; set; }

        public int Difficulty { get; set; }

        public string PreviousBlockHash { get; set; }

        public string MinedBy { get; set; }

        public byte[] DataHash
        {
            get
            {
                if (_dataHash == null)
                {
                    _dataHash = ComputeDataHash();
                }

                return _dataHash;
            }
        }

        public ulong Nonce { get; set; }

        public DateTime DateCreated { get; set; }

        public string MinerProvidedHash { get; set; }

        private byte[] ComputeDataHash()
        {
            BlockData blockData = new BlockData
            {
                Index = Index,
                Transactions = Transactions.Select(t => new TransactionData(t)).ToArray(),
                Difficulty = Difficulty,
                PreviousBlockHash = IsGenesis ? null : PreviousBlockHash,
                MinedBy = MinedBy
            };
            string json = JsonUtils.Serialize(blockData, false);
            return HashUtils.ComputeSha256(json.GetBytes());
        }
    }
}
