using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Pickaxe.Blockchain.Domain
{
    public class NodeService : INodeService
    {
        private INodeSettings _nodeSettings;
        private ITransactionService _transactionService;

        private BlockingCollection<Block> _blockchain;
        private ConcurrentDictionary<string, Transaction> _pendingTransactions;
        private ConcurrentDictionary<string, Block> _miningJobs;

        public NodeService(
            INodeSettings nodeSettings,
            ITransactionService transactionService)
        {
            _nodeSettings = nodeSettings;
            _transactionService = transactionService;
            _blockchain = new BlockingCollection<Block>()
            {
                Block.GenesisBlock
            };
            _pendingTransactions = new ConcurrentDictionary<string, Transaction>();
            _miningJobs = new ConcurrentDictionary<string, Block>();
        }

        public Block CreateCandidateBlock(string minerAddress)
        {
            Block blockCandidate = new Block
            {
                Index = _blockchain.Count,
                Difficulty = _nodeSettings.Difficulty,
                PreviousBlockHash = _blockchain.Last().DataHash,
                MinedBy = minerAddress,
                Nonce = 0,
                DateCreated = DateTime.UtcNow
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

        public BlockValidationResult TryAddBlock(MiningResult miningResult, out Block candidateBlock)
        {
            bool found = _miningJobs.TryGetValue(miningResult.MinerAddress, out candidateBlock);
            if (!found)
            {
                return BlockValidationResult.MinerAddressNotFound;
            }

            if (candidateBlock.DataHash.ToHex() != miningResult.BlockDataHash)
            {
                return BlockValidationResult.BlockDataHashMismatch;
            }

            string difficultyCheck = new string('0', _nodeSettings.Difficulty);
            if (!miningResult.BlockHash.StartsWith(difficultyCheck))
            {
                return BlockValidationResult.BlockHashDifficultyMismatch;
            }

            string blockHashCheck = HashUtils.ComputeBlockSha256Hash(
                miningResult.BlockDataHash,
                miningResult.DateCreated,
                miningResult.Nonce);
            if (blockHashCheck != miningResult.BlockHash)
            {
                return BlockValidationResult.InvalidBlockHash;
            }

            if (candidateBlock.Index != _blockchain.Count)
            {
                return BlockValidationResult.BlockAlreadyAdded;
            }

            _blockchain.Add(candidateBlock);

            candidateBlock.MinerProvidedHash = miningResult.BlockHash;
            candidateBlock.DateCreated = miningResult.DateCreated;
            candidateBlock.Nonce = miningResult.Nonce;

            return BlockValidationResult.Ok;
        }

        public Block GetBlock(int index)
        {
            return _blockchain.ElementAtOrDefault(index);
        }
    }
}
