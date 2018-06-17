using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Domain
{
    public interface INodeService
    {
        Block CreateBlockCandidate(string minerAddress, decimal expectedReward);
    }
}