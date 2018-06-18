using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Signer;
using Pickaxe.Blockchain.Domain.Models;
using Pickaxe.Blockchain.Domain.Serialization;

namespace Pickaxe.Blockchain.Domain
{
    public class TransactionService : ITransactionService
    {
        public bool Verify(Models.Transaction transaction)
        {
            byte[] transactionDataHash = transaction.DataHash;

            EthECKey publicKey = EthECKey.RecoverFromSignature(
                transaction.SenderSignature,
                transactionDataHash);
            bool valid = publicKey.GetPublicAddress() == transaction.From;
            return valid;
        }
    }
}
