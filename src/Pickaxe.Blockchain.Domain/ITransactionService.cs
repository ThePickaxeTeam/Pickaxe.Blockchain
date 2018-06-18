namespace Pickaxe.Blockchain.Domain
{
    public interface ITransactionService
    {
        bool Verify(Models.Transaction transaction);
    }
}