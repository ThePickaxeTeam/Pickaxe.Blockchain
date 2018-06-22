using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Domain.Serialization;
using System;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class Transaction
    {
        private byte[] _dataHash;

        public string From { get; set; }

        public string To { get; set; }

        public long Value { get; set; }

        public long Fee { get; set; }

        public DateTime DateCreated { get; set; }

        public string Data { get; set; }

        public string SenderPublicKey { get; set; }

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

        public string[] SenderSignature { get; set; }

        public int MinedInBlockIndex { get; set; }

        public bool TransferSuccessful { get; set; }

        public bool Confirmed { get; set; }

        public int Confirmations { get; set; }

        public bool IncludedInSafeBalance { get; set; }

        public static Transaction CreateCoinbaseTransaction(
            string minerAddress,
            int blockIndex)
        {
            return new Transaction
            {
                From = new string('0', 40),
                To = minerAddress,
                Value = 5000350,
                Fee = 0,
                DateCreated = DateTime.UtcNow,
                Data = "coinbase tx",
                SenderPublicKey = new string('0', 65),
                SenderSignature = new string[]
                {
                    new string('0', 64),
                    new string('0', 64)
                },
                MinedInBlockIndex = blockIndex,
                TransferSuccessful = true
            };
        }

        private byte[] ComputeDataHash()
        {
            string json = JsonUtils.Serialize(new TransactionDataBase(this), false);
            return HashUtils.ComputeSha256(json.GetBytes());
        }
    }
}
