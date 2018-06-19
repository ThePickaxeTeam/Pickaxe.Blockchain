using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
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
        private static Block _genesisBlock;
        private byte[] _dataHash;

        public Block()
        {
            Transactions = new List<Transaction>();
        }

        public static Block GenesisBlock
        {
            get
            {
                if (_genesisBlock == null)
                {
                    _genesisBlock = new Block
                    {
                        Index = 0,
                        Transactions = new List<Transaction>
                        {
                            new Transaction
                            {
                                From = (new byte[20]).ToHex(),
                                To = "f3a1e69b6176052fcc4a3248f1c5a91dea308ca9",
                                Value = 1000000000000,
                                Fee = 0,
                                DateCreated = new DateTime(2018, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                Data = "genesis tx",
                                SenderPublicKey = (new string('0', 65)).HexToByteArray(),
                                SenderSignature = EthECDSASignatureFactory.FromComponents(
                                    new byte[32],
                                    new byte[32],
                                    0),
                                MinedInBlockIndex = 0,
                                TransferSuccessful = true
                            }
                        },
                        Difficulty = 0,
                        PreviousBlockHash = null,
                        MinedBy = (new byte[20]).ToHex(),
                        Nonce = 0,
                        DateCreated = DateTime.UtcNow,
                        MinerProvidedHash = "c6da93eb4249cb5ff4f9da36e2a7f8d0d61999221ed6910180948153e71cc47f"
                    };
                }
                return _genesisBlock;
            }
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

        public byte[] PreviousBlockHash { get; set; }

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
                PreviousBlockHash = IsGenesis ? null : PreviousBlockHash.ToHex(),
                MinedBy = MinedBy
            };
            string json = JsonUtils.Serialize(blockData, false);
            return HashUtils.ComputeSha256(json.GetBytes());
        }
    }
}
