using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Generic;

namespace Pickaxe.Blockchain.Domain
{
    public interface INodeService
    {
        Block CreateCandidateBlock(string minerAddress);

        BlockValidationResult TryAddBlock(MinedBlock minedBlock, out Block candidateBlock);

        CreateTransactionResult TryCreateTransaction(Transaction transaction);

        bool TryGetTransaction(string transactionDataHash, out Transaction transaction);

        Dictionary<string, long> GetAllBalances();

        bool TryGetAccountBalances(string address, out AccountBalances balances);

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