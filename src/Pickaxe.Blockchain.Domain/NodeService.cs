using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Pickaxe.Blockchain.Domain
{
    public class NodeService : INodeService
    {
        private INodeSettings _nodeSettings;
        private ITransactionService _transactionService;
        private IPeersUpdateService _peersUpdateService;

        private BlockingCollection<Block> _blockchain;
        private ConcurrentDictionary<string, Transaction> _pendingTransactions;
        private ConcurrentDictionary<string, Transaction> _confirmedTransactions;
        private ConcurrentDictionary<string, Block> _miningJobs;

        public NodeService(
            INodeSettings nodeSettings,
            ITransactionService transactionService,
            IPeersUpdateService peersUpdateService)
        {
            _nodeSettings = nodeSettings;
            _transactionService = transactionService;
            _peersUpdateService = peersUpdateService;
            _blockchain = new BlockingCollection<Block>()
            {
                Block.GenesisBlock
            };
            _confirmedTransactions = new ConcurrentDictionary<string, Transaction>();
            AddFaucetTransactionAsConfirmed();

            _pendingTransactions = new ConcurrentDictionary<string, Transaction>();
            _miningJobs = new ConcurrentDictionary<string, Block>();
        }

        public Block CreateCandidateBlock(string minerAddress)
        {
            Block blockCandidate = new Block
            {
                Index = _blockchain.Count,
                Difficulty = _nodeSettings.CurrentDifficulty,
                PreviousBlockHash = _blockchain.Last().DataHash,
                MinedBy = minerAddress
            };

            int blockIndex = _blockchain.Count;
            blockCandidate.Transactions.Add(
                Transaction.CreateCoinbaseTransaction(
                    minerAddress,
                    blockIndex));

            foreach (Transaction transaction in _pendingTransactions.Values)
            {
                transaction.MinedInBlockIndex = blockIndex;
                transaction.TransferSuccessful = true;
                blockCandidate.Transactions.Add(transaction);
            }

            _miningJobs.AddOrUpdate(
                blockCandidate.DataHash.ToHex(),
                blockCandidate,
                (address, oldCandidate) => blockCandidate);

            return blockCandidate;
        }

        public BlockValidationResult TryAddBlock(MiningResult miningResult, out Block candidateBlock)
        {
            bool found = _miningJobs.TryGetValue(miningResult.BlockDataHash, out candidateBlock);
            if (!found)
            {
                return BlockValidationResult.BlockNotFound;
            }

            string difficultyCheck = new string('0', _nodeSettings.CurrentDifficulty);
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
                return BlockValidationResult.BlockAlreadyMined;
            }

            // block found, will be added in chain
            _blockchain.Add(candidateBlock);
            _miningJobs.Clear();

            UpdateCandidateBlockData(candidateBlock, miningResult);
            UpdateTransactionsData(candidateBlock);

            return BlockValidationResult.Ok;
        }

        public IList<Transaction> GetPendingTransactions()
        {
            return _pendingTransactions.Values.ToList();
        }

        public IList<Transaction> GetConfirmedTransactions()
        {
            return _confirmedTransactions.Values.ToList();
        }

        public IList<Block> GetAllBlocks()
        {
            return _blockchain.ToList();
        }

        public Block GetBlock(int index)
        {
            return _blockchain.ElementAtOrDefault(index);
        }

        public NodeInfo GetNodeInfo()
        {
            return new NodeInfo
            {
                About = _nodeSettings.About,
                NodeId = _nodeSettings.NodeId,
                ChainId = Block.GenesisBlock.DataHash.ToHex(),
                NodeUrl = _nodeSettings.NodeUrl,
                Peers = _peersUpdateService.GetPeersCount(),
                CurrentDifficulty = _nodeSettings.CurrentDifficulty,
                BlocksCount = _blockchain.Count,
                CumulativeDifficulty = _blockchain.Sum(b => b.Difficulty),
                ConfirmedTransactions = 0, // TODO:
                PendingTransactions = _pendingTransactions.Count
            };
        }

        public DebugInfo GetDebugInfo()
        {
            return new DebugInfo
            {
                SelfUrl = _nodeSettings.NodeUrl,
                Blocks = _blockchain.ToList()
            };
        }

        public void ResetChain()
        {
            _blockchain.Dispose();
            _blockchain = new BlockingCollection<Block>()
            {
                Block.GenesisBlock
            };
            _confirmedTransactions.Clear();
            AddFaucetTransactionAsConfirmed();

            _pendingTransactions.Clear();
            _miningJobs.Clear();
        }

        private void UpdateTransactionsData(Block candidateBlock)
        {
            foreach (Transaction transaction in candidateBlock.Transactions)
            {
                _confirmedTransactions.TryAdd(transaction.DataHash.ToHex(), transaction);
                _pendingTransactions.TryRemove(transaction.DataHash.ToHex(), out _);
            }
        }

        private void AddFaucetTransactionAsConfirmed()
        {
            Transaction faucetTransaction = Block.GenesisBlock.Transactions[0];
            _confirmedTransactions.TryAdd(faucetTransaction.DataHash.ToHex(), faucetTransaction);
        }

        private static void UpdateCandidateBlockData(
            Block candidateBlock,
            MiningResult miningResult)
        {
            candidateBlock.MinerProvidedHash = miningResult.BlockHash;
            candidateBlock.DateCreated = miningResult.DateCreated;
            candidateBlock.Nonce = miningResult.Nonce;
        }
    }
}
