using Nethereum.Hex.HexConvertors.Extensions;

namespace Pickaxe.Blockchain.Domain.Serialization
{
    public class TransactionDataBase
    {
        public TransactionDataBase(Models.Transaction transaction)
        {
            From = transaction.From;
            To = transaction.To;
            Value = transaction.Value;
            Fee = transaction.Fee;
            DateCreated = transaction.DateCreated.ToString("o");
            Data = transaction.Data;
            SenderPublicKey = transaction.SenderPublicKey.ToHex();
        }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Value { get; set; }

        public decimal Fee { get; set; }

        public string DateCreated { get; set; }

        public string Data { get; set; }

        public string SenderPublicKey { get; set; }
    }
}
