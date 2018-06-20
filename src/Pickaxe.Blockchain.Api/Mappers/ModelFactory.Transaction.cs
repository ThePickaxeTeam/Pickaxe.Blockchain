﻿using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Domain.Models;
using TransactionContract = Pickaxe.Blockchain.Contracts.Transaction;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static TransactionContract ToContract(this Transaction transaction)
        {
            return new TransactionContract
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
                },
                MinedInBlockIndex = transaction.MinedInBlockIndex,
                TransferSuccessful = transaction.TransferSuccessful
            };
        }
    }
}
