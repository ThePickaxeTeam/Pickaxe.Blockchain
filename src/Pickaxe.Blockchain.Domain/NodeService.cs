using Pickaxe.Blockchain.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Pickaxe.Blockchain.Domain
{
    public class NodeService : INodeService
    {
        private const int Difficulty = 4;

        private ITransactionService _transactionService;

        private BlockingCollection<Block> _blockchain;
        private ConcurrentDictionary<string, Transaction> _pendingTransactions;
        private ConcurrentDictionary<string, Block> _miningJobs;

        public NodeService(ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _blockchain = new BlockingCollection<Block>()
            {
                Block.GenesisBlock
            };
            _pendingTransactions = new ConcurrentDictionary<string, Transaction>();
            _miningJobs = new ConcurrentDictionary<string, Block>();
        }

        public Block CreateCandidateBlock(
            string minerAddress)
        {
            Block blockCandidate = new Block
            {
                Index = _blockchain.Count,
                Difficulty = Difficulty,
                PreviousBlockHash = _blockchain.Last().DataHash,
                MinedBy = minerAddress,
                Nonce = 0,
                CreatedAtUtc = DateTime.UtcNow
            };

            blockCandidate.Transactions.Add(
                Transaction.CreateCoinbaseTransaction(
                    minerAddress,
                    _blockchain.Count));

            foreach (Transaction transaction in _pendingTransactions.Values)
            {
                blockCandidate.Transactions.Add(transaction);
            }

            _miningJobs.AddOrUpdate(
                minerAddress, 
                blockCandidate, 
                (address, oldCandidate) => blockCandidate);

            return blockCandidate;
        }
    }
}
