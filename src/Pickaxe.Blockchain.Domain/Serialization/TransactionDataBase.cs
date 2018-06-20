using Newtonsoft.Json;
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
            SenderPubKey = transaction.SenderPublicKey;
        }

        [JsonProperty(Order = 1)]
        public string From { get; set; }

        [JsonProperty(Order = 2)]
        public string To { get; set; }

        [JsonProperty(Order = 3)]
        public long Value { get; set; }

        [JsonProperty(Order = 4)]
        public long Fee { get; set; }

        [JsonProperty(Order = 5)]
        public string DateCreated { get; set; }

        [JsonProperty(Order = 6)]
        public string Data { get; set; }

        [JsonProperty(Order = 7)]
        public string SenderPubKey { get; set; }
    }
}
