using Nethereum.Hex.HexConvertors.Extensions;
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
                transaction.SenderSignature.R.ToHex(),
                transaction.SenderSignature.S.ToHex()
            };
            MinedInBlockIndex = transaction.MinedInBlockIndex;
            TransferSuccessful = transaction.TransferSuccessful;
        }

        public string TransactionDataHash { get; set; }

        public string[] SenderSignature { get; set; }

        public int MinedInBlockIndex { get; set; }

        public bool TransferSuccessful { get; set; }
    }
}
