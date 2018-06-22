using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pickaxe.Blockchain.Domain
{
    public interface INodeService
    {
        Block CreateCandidateBlock(string minerAddress);

        BlockValidationResult TryAddBlock(MiningResult miningResult, out Block candidateBlock);

        CreateTransactionResult TryCreateTransaction(Transaction transaction);

        bool TryGetTransaction(string transactionDataHash, out Transaction transaction);

        ReadOnlyDictionary<string, long> GetAllBalances();

        IEnumerable<Transaction> GetTransactions(string address);

        IList<Transaction> GetPendingTransactions();

        IList<Transaction> GetConfirmedTransactions();

        IList<Block> GetAllBlocks();

        Block GetBlock(int index);

        NodeInfo GetNodeInfo();

        DebugInfo GetDebugInfo();

        void ResetChain();
    }
}