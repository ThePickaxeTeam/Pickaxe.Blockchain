using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Domain
{
    public interface INodeService
    {
        Block CreateCandidateBlock(string minerAddress);
    }
}