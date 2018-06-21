using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Contracts;
using System;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;
using TransactionContract = Pickaxe.Blockchain.Contracts.Transaction;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static TransactionContract ToContract(
            this Transaction transaction,
            bool confirmed = true)
        {
            var contract = new TransactionContract
            {
                From = transaction.From,
                To = transaction.To,
                Value = transaction.Value,
                Fee = transaction.Fee,
                DateCreated = transaction.DateCreated.Iso8601Formatted(),
                Data = transaction.Data,
                SenderPubKey = transaction.SenderPublicKey,
                TransactionDataHash = transaction.DataHash.ToHex(),
                SenderSignature = new string[]
                {
                    transaction.SenderSignature[0],
                    transaction.SenderSignature[1]
                }
            };

            if (confirmed)
            {
                contract.MinedInBlockIndex = transaction.MinedInBlockIndex;
                contract.TransferSuccessful = transaction.TransferSuccessful;
            }

            return contract;
        }

        public static Transaction ToDomainModel(this CreateTransactionRequest request)
        {
            return new Transaction
            {
                From = request.From,
                To = request.To,
                Value = request.Value,
                Fee = request.Fee,
                DateCreated = DateTime.Parse(request.DateCreated).ToUniversalTime(),
                Data = request.Data,
                SenderPublicKey= request.SenderPubKey,
                SenderSignature = new string[]
                {
                    request.SenderSignature[0],
                    request.SenderSignature[1]
                }
            };
        }
    }
}
