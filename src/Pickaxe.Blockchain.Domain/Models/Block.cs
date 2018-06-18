using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Pickaxe.Blockchain.Common;
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
                                To = (new byte[20]).ToHex(),
                                Value = 1000,
                                Fee = 0,
                                CreatedAtUtc = DateTime.UtcNow,
                                Data = "Genesis block transaction",
                                SenderPublicKey = new byte[64],
                                SenderSignature = EthECDSASignatureFactory.FromComponents(
                                    new byte[32],
                                    new byte[32],
                                    0),
                                MinedInBlockIndex = 0,
                                TransferSuccessful = true
                            }
                        },
                        MinedBy = (new byte[20]).ToHex(),
                        CreatedAtUtc = DateTime.UtcNow,
                        PreviousBlockHash = new byte[32]
                    };
                }
                return _genesisBlock;
            }
        }

        public int Index { get; set; }

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
                    _dataHash = ComputeHash();
                }

                return _dataHash;
            }
        }

        public ulong Nonce { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        private byte[] ComputeHash()
        {
            BlockData blockData = new BlockData
            {
                Index = Index,
                Transactions = Transactions.Select(t => new TransactionData(t)).ToArray(),
                Difficulty = Difficulty,
                PreviousBlockHash = PreviousBlockHash.ToHex(),
                MinedBy = MinedBy
            };
            string json = JsonUtils.Serialize(blockData, false);
            return HashUtils.ComputeSha256(Utils.GetBytes(json));
        }
    }
}
