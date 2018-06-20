using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Domain
{
    public interface IPeersUpdateService
    {
        int GetPeersCount();

        void UpdatePeers(Block block);
    }
}