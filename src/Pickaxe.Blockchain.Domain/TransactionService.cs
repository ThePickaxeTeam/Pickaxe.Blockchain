using Nethereum.Signer;
using Pickaxe.Blockchain.Common;

namespace Pickaxe.Blockchain.Domain
{
    public class TransactionService : ITransactionService
    {
        public bool Verify(Models.Transaction transaction)
        {
            byte[] transactionDataHash = transaction.DataHash;

            EthECDSASignature signature = EncryptionUtils.GetEthECDSASignature(
                transaction.SenderSignature);
            EthECKey publicKey = EthECKey.RecoverFromSignature(
                signature,
                transactionDataHash);
            bool valid = publicKey.GetPublicAddress() == transaction.From;
            return valid;
        }
    }
}
