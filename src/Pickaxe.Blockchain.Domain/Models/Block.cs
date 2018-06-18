using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Signer.Crypto;
using Org.BouncyCastle.Math;
using Pickaxe.Blockchain.Common;
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

        public static Block CreateGenesisBlock(string to)
        {
            string data = "Genesis block transaction";
            Block genesisBlock = new Block
            {
                Index = 0,
                Transactions = new List<Transaction>
                {
                    new Transaction
                    {
                        From = (new byte[20]).ToHex(),
                        To = to,
                        Value = 1000,
                        Fee = 0,
                        CreatedAtUtc = DateTime.UtcNow,
                        Data = data,
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

            return genesisBlock;
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

        public byte[] Hash { get; set; }

        private byte[] ComputeHash()
        {
            BlockData blockData = new BlockData
            {
                Index = Index,
                Transactions = Transactions.Select(t => new TransactionData
                {
                    From = t.From,
                    To = t.To,
                    Value = t.Value,
                    Fee = t.Fee,
                    DateCreated = t.CreatedAtUtc.ToString("o"),
                    Data = t.Data,
                    SenderPublicKey = t.SenderPublicKey.ToHex(),
                    DataHash = t.DataHash.ToHex(),
                    SenderSignature = new string[]
                    {
                        t.SenderSignature.R.ToHex(),
                        t.SenderSignature.S.ToHex()
                    },
                    MinedInBlockIndex = t.MinedInBlockIndex,
                    TransferSuccessful = t.TransferSuccessful
                }).ToArray(),
                Difficulty = Difficulty,
                PreviousBlockHash = PreviousBlockHash.ToHex(),
                MinedBy = MinedBy
            };
            string json = JsonUtils.Serialize(blockData, false);
            return HashUtils.ComputeSha256(Utils.GetBytes(json));
        }
    }
}
