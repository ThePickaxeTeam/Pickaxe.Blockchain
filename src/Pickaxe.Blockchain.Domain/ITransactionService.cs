using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;

namespace Pickaxe.Blockchain.Domain
{
    public interface ITransactionService
    {
        bool ValidateSignature(Transaction transaction);
    }
}