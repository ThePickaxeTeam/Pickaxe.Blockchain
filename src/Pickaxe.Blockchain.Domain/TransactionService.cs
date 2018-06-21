﻿using Nethereum.Signer;
using Pickaxe.Blockchain.Common;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;


namespace Pickaxe.Blockchain.Domain
{
    public class TransactionService : ITransactionService
    {
        public bool ValidateSignature(Transaction transaction)
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
