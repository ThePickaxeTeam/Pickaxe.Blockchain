using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Contracts;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;

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
    }
}
