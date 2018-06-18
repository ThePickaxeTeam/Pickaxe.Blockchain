using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Nethereum.Signer.Crypto;
using Org.BouncyCastle.Math;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Domain.Serialization;
using System;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class Transaction
    {
        private byte[] _dataHash;

        public string From { get; set; }

        public string To { get; set; }

        public decimal Value { get; set; }

        public decimal Fee { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public string Data { get; set; }

        public byte[] SenderPublicKey { get; set; }

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

        public EthECDSASignature SenderSignature { get; set; }

        public int MinedInBlockIndex { get; set; }

        public bool TransferSuccessful { get; set; }

        private byte[] ComputeHash()
        {
            TransactionData transactionData = new TransactionData
            {
                From = From,
                To = To,
                Value = Value,
                Fee = Fee,
                DateCreated = CreatedAtUtc.ToString("o"),
                Data = Data,
                SenderPublicKey = SenderPublicKey.ToHex(),
            };
            string json = JsonUtils.Serialize(transactionData, false);
            return HashUtils.ComputeSha256(Utils.GetBytes(json));
        }

        public static Transaction CreateCoinbaseTransaction(
            string minerAddress,
            decimal expectedReward,
            int blockIndex)
        {
            return new Transaction
            {
                From = (new byte[20]).ToHex(),
                To = minerAddress,
                Value = expectedReward,
                SenderPublicKey = new byte[64],
                SenderSignature = EthECDSASignatureFactory.FromComponents(
                    new byte[32],
                    new byte[32],
                    0),
                Data = "Coinbase transaction",
                CreatedAtUtc = DateTime.UtcNow,
                MinedInBlockIndex = blockIndex
            };
        }
    }
}
