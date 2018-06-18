using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
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

        public static Transaction CreateCoinbaseTransaction(
            string minerAddress,
            int blockIndex)
        {
            return new Transaction
            {
                From = (new byte[20]).ToHex(),
                To = minerAddress,
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

        private byte[] ComputeHash()
        {
            string json = JsonUtils.Serialize(new TransactionDataBase(this), false);
            return HashUtils.ComputeSha256(Utils.GetBytes(json));
        }
    }
}
