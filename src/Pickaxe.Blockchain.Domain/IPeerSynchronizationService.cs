using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Domain
{
    public interface IPeerSynchronizationService
    {
        Dictionary<string, string> GetPeers();

        int GetPeersCount();

        Task BroadcastNewBlockNotification(
            int blocksCount,
            long cumulativeDifficulty,
            string nodeUrl);

        Task BroadcastNewTransaction(Transaction transaction);

        Task<List<Block>> GetAllBlocks(string peerUrl);
    }
}
