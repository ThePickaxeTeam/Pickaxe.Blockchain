using Nethereum.Signer;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Pickaxe.Blockchain.Common;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;


namespace Pickaxe.Blockchain.Domain
{
    public class TransactionService : ITransactionService
    {
        public bool VerifySignature(Transaction transaction)
        {
            byte[] transactionDataHash = transaction.DataHash;
            BigInteger r = new BigInteger(transaction.SenderSignature[0], 16);
            BigInteger s = new BigInteger(transaction.SenderSignature[1], 16);
            ECPoint publicKey = EncryptionUtils.DecompressKey(transaction.SenderPublicKey);

            bool signatureVerified = EncryptionUtils.VerifySignature(
                publicKey, r, s, transactionDataHash);
            return signatureVerified;
        }
    }
}
