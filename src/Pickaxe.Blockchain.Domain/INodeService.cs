using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Generic;

namespace Pickaxe.Blockchain.Domain
{
    public interface INodeService
    {
        Block CreateCandidateBlock(string minerAddress);

        BlockValidationResult TryAddBlock(MiningResult miningResult, out Block candidateBlock);

        bool TryGetTransaction(string transactionDataHash, out Transaction transaction);

        IList<Transaction> GetPendingTransactions();

        IList<Transaction> GetConfirmedTransactions();

        IList<Block> GetAllBlocks();

        Block GetBlock(int index);

        NodeInfo GetNodeInfo();

        DebugInfo GetDebugInfo();

        void ResetChain();
    }
}