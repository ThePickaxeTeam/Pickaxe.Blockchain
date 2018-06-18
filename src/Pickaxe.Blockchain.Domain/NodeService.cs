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
            _blockchain = new BlockingCollection<Block>();
            _pendingTransactions = new ConcurrentDictionary<string, Transaction>();
            _miningJobs = new ConcurrentDictionary<string, Block>();
        }

        public Block CreateBlockCandidate(
            string minerAddress,
            decimal expectedReward)
        {
            Block blockCandidate = new Block
            {
                Index = _blockchain.Count,
                Difficulty = Difficulty,
                PreviousBlockHash = _blockchain.ElementAt(_blockchain.Count - 1).Hash,
                MinedBy = minerAddress,
                Nonce = 0,
                CreatedAtUtc = DateTime.UtcNow,
                Hash = new byte[32]
            };

            blockCandidate.Transactions.Add(
                Transaction.CreateCoinbaseTransaction(
                    minerAddress,
                    expectedReward,
                    _blockchain.Count));
            foreach (Transaction transaction in _pendingTransactions.Values)
            {
                blockCandidate.Transactions.Add(transaction);
            }

            return blockCandidate;
        }
    }
}
