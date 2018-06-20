using Nethereum.Hex.HexConvertors.Extensions;
using Newtonsoft.Json;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Domain.Serialization
{
    public class TransactionData : TransactionDataBase
    {
        public TransactionData(Transaction transaction)
            : base(transaction)
        {
            TransactionDataHash = transaction.DataHash.ToHex();
            SenderSignature = new string[]
            {
                transaction.SenderSignature[0],
                transaction.SenderSignature[1]
            };
            MinedInBlockIndex = transaction.MinedInBlockIndex;
            TransferSuccessful = transaction.TransferSuccessful;
        }

        [JsonProperty(Order = 8)]
        public string TransactionDataHash { get; set; }

        [JsonProperty(Order = 9)]
        public string[] SenderSignature { get; set; }

        [JsonProperty(Order = 10)]
        public int MinedInBlockIndex { get; set; }

        [JsonProperty(Order = 11)]
        public bool TransferSuccessful { get; set; }
    }
}
