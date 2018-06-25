using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Contracts;
using System;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;
using TransactionContract = Pickaxe.Blockchain.Contracts.Transaction;


namespace Pickaxe.Blockchain.Domain.Mappers
{
    public static partial class ModelFactory
    {
        public static CreateTransactionRequest ToCreateRequest(this Transaction transaction)
        {
            return new CreateTransactionRequest
            {
                From = transaction.From,
                To = transaction.To,
                Value = transaction.Value,
                Fee = transaction.Fee,
                DateCreated = transaction.DateCreated.Iso8601Formatted(),
                Data = transaction.Data,
                SenderPubKey = transaction.SenderPublicKey,
                SenderSignature = new string[]
                {
                    transaction.SenderSignature[0],
                    transaction.SenderSignature[1]
                }
            };
        }

        public static Transaction ToDomainModel(this TransactionContract contract)
        {
            return new Transaction
            {
                From = contract.From,
                To = contract.To,
                Value = contract.Value,
                Fee = contract.Fee,
                DateCreated = DateTime.Parse(contract.DateCreated).ToUniversalTime(),
                Data = contract.Data,
                SenderPublicKey = contract.SenderPubKey,
                SenderSignature = new string[]
                {
                    contract.SenderSignature[0],
                    contract.SenderSignature[1]
                }
            };
        }
    }
}
