using Nethereum.Hex.HexConvertors.Extensions;
using System;
using System.Collections.Generic;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class Transaction
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Value { get; set; }

        public string SenderPublicKey { get; set; }

        public string SenderSignature { get; set; }

        public DateTime ReceivedAtUtc { get; set; }

        public decimal Fee { get; set; }

        public int BlockIndex { get; set; }

        public string Hash { get; set; }

        public static Transaction CreateCoinbaseTransaction(
            string minerAddress,
            decimal expectedReward,
            int blockIndex)
        {
            byte[] from = new byte[20];
            return new Transaction
            {
                From = from.ToHex(),
                To = minerAddress,
                Value = expectedReward,
                SenderPublicKey = string.Empty,
                SenderSignature = string.Empty,
                Hash = string.Empty,
                ReceivedAtUtc = DateTime.UtcNow,
                BlockIndex = blockIndex
            };
        }
    }
}
