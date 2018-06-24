using Pickaxe.Blockchain.Contracts;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Domain
{
    public interface IPeerSynchronizationService
    {
        int GetPeersCount();

        Task BroadcastNewBlockNotification(
            int blocksCount,
            long cumulativeDifficulty,
            string nodeUrl);

        Task BroadcastNewTransaction(CreateTransactionRequest request);
    }
}
