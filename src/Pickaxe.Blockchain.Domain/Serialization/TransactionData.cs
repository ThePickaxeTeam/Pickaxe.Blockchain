using Nethereum.Hex.HexConvertors.Extensions;

namespace Pickaxe.Blockchain.Domain.Serialization
{
    public class TransactionData : TransactionDataBase
    {
        public TransactionData(Models.Transaction transaction)
            : base(transaction)
        {
            DataHash = transaction.DataHash.ToHex();
            SenderSignature = new string[]
            {
                transaction.SenderSignature.R.ToHex(),
                transaction.SenderSignature.S.ToHex()
            };
            MinedInBlockIndex = transaction.MinedInBlockIndex;
            TransferSuccessful = transaction.TransferSuccessful;
        }

        public string DataHash { get; set; }

        public string[] SenderSignature { get; set; }

        public int MinedInBlockIndex { get; set; }

        public bool TransferSuccessful { get; set; }
    }
}
