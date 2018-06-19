using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;

namespace Pickaxe.Blockchain.Domain
{
    public interface INodeService
    {
        Block CreateCandidateBlock(string minerAddress);

        BlockValidationResult TryAddBlock(MiningResult miningResult, out Block candidateBlock);

        Block GetBlock(int index);
    }
}