using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Domain.Serialization
{
    public class TransactionDataBase
    {
        public TransactionDataBase(Transaction transaction)
        {
            From = transaction.From;
            To = transaction.To;
            Value = transaction.Value;
            Fee = transaction.Fee;
            DateCreated = transaction.DateCreated.Iso8601Formatted();
            Data = transaction.Data;
            SenderPubKey = transaction.SenderPublicKey.ToHex();
        }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Value { get; set; }

        public decimal Fee { get; set; }

        public string DateCreated { get; set; }

        public string Data { get; set; }

        public string SenderPubKey { get; set; }
    }
}
