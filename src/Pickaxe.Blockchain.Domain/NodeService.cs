using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Domain.Enums;
using Pickaxe.Blockchain.Domain.Models;
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
        private ConcurrentDictionary<string, AccountBalances> _accountBalances;
        private ConcurrentDictionary<string, Block> _miningJobs;

        public NodeService(
            INodeSettings nodeSettings,
            ITransactionService transactionService,
            IPeersUpdateService peersUpdateService)
        {
            _nodeSettings = nodeSettings;
            _transactionService = transactionService;
            _peersUpdateService = peersUpdateService;
            Block genesisBlock = Block.CreateGenesisBlock();
            _blockchain = new BlockingCollection<Block>() { genesisBlock };
            _confirmedTransactions = new ConcurrentDictionary<string, Transaction>();
            _accountBalances = new ConcurrentDictionary<string, AccountBalances>();
            AddGenesisTransactionsAsConfirmed(genesisBlock.Transactions);

            _pendingTransactions = new ConcurrentDictionary<string, Transaction>();
            _miningJobs = new ConcurrentDictionary<string, Block>();
        }

        public Block CreateCandidateBlock(string minerAddress)
        {
            Block blockCandidate = new Block
            {
                Index = _blockchain.Count,
                Difficulty = _nodeSettings.CurrentDifficulty,
                PreviousBlockHash = _blockchain.Last().MinerProvidedHash,
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
            MoveBlockTransactionsToConfirmed(candidateBlock);

            return BlockValidationResult.Ok;
        }

        public CreateTransactionResult TryCreateTransaction(Transaction transaction)
        {
            bool signatureVerified = _transactionService.VerifySignature(transaction);
            if (!signatureVerified)
            {
                return CreateTransactionResult.VerificationFailed;
            }

            bool transactionExists = CheckTransactionExists(transaction.DataHash.ToHex());
            if (transactionExists)
            {
                return CreateTransactionResult.DuplicateTransaction;
            }

            long senderBalance = GetAccountBalance(transaction.From);
            if (senderBalance < transaction.Value + transaction.Fee)
            {
                return CreateTransactionResult.InsufficientBalance;
            }

            if (transaction.Fee < _nodeSettings.MinFee)
            {
                return CreateTransactionResult.InsufficientFee;
            }

            _pendingTransactions.TryAdd(transaction.DataHash.ToHex(), transaction);
            UpdatePendingAccountBalances(transaction);

            return CreateTransactionResult.Ok;
        }

        public bool TryGetTransaction(string transactionDataHash, out Transaction transaction)
        {
            if (_pendingTransactions.TryGetValue(transactionDataHash, out transaction))
            {
                transaction.Confirmed = false;
                return true;
            }
            if (_confirmedTransactions.TryGetValue(transactionDataHash, out transaction))
            {
                transaction.Confirmed = true;
                return true;
            }

            return false;
        }

        public Dictionary<string, long> GetAllBalances()
        {
            return _accountBalances.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ConfirmedBalance);
        }

        public bool TryGetAccountBalances(string address, out AccountBalances balances)
        {
            if (_accountBalances.ContainsKey(address))
            {
                balances = _accountBalances[address];
                return true;
            }
            else
            {
                balances = null;
                return false;
            }
        }

        public IEnumerable<Transaction> GetTransactions(string address)
        {
            List<Transaction> matchingPendingTransactions =
                GetMatchingTransactions(_pendingTransactions.Values, address, false);
            List<Transaction> matchingConfirmedTransactions =
                GetMatchingTransactions(_confirmedTransactions.Values, address, true);
            IEnumerable<Transaction> result =
                matchingPendingTransactions.Concat(matchingConfirmedTransactions);

            return result;
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
                ChainId = _blockchain.ElementAt(0).DataHash.ToHex(),
                NodeUrl = _nodeSettings.NodeUrl,
                Peers = _peersUpdateService.GetPeersCount(),
                CurrentDifficulty = _nodeSettings.CurrentDifficulty,
                BlocksCount = _blockchain.Count,
                CumulativeDifficulty = _blockchain.Sum(b => b.Difficulty),
                ConfirmedTransactions = _confirmedTransactions.Count,
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
            Block genesisBlock = Block.CreateGenesisBlock();
            _blockchain = new BlockingCollection<Block>() { genesisBlock };
            _confirmedTransactions.Clear();
            _accountBalances.Clear();
            AddGenesisTransactionsAsConfirmed(genesisBlock.Transactions);

            _pendingTransactions.Clear();
            _miningJobs.Clear();
        }

        private void MoveBlockTransactionsToConfirmed(Block candidateBlock)
        {
            foreach (Transaction confirmedTransaction in _confirmedTransactions.Values)
            {
                confirmedTransaction.Confirmations++;
            }

            foreach (Transaction transaction in candidateBlock.Transactions)
            {
                transaction.Confirmations = 1;

                _confirmedTransactions.TryAdd(transaction.DataHash.ToHex(), transaction);
                _pendingTransactions.TryRemove(transaction.DataHash.ToHex(), out _);
            }

            UpdateSafeAccountBalances();
            UpdateConfirmedAccountBalances(candidateBlock.Transactions);
            RevertPendingAccountBalances(candidateBlock.Transactions);
        }

        private void AddGenesisTransactionsAsConfirmed(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                transaction.Confirmations = 1;

                _confirmedTransactions.TryAdd(
                    transaction.DataHash.ToHex(),
                    transaction);
            }
            UpdateConfirmedAccountBalances(transactions);
        }

        private void UpdateSafeAccountBalances()
        {
            foreach (Transaction transaction in _confirmedTransactions.Values)
            {
                AddAccountBalancesIfMissing(transaction);

                if (transaction.Confirmations >= _nodeSettings.SafeBalanceConfirmations &&
                    !transaction.IncludedInSafeBalance)
                {
                    _accountBalances[transaction.From].SafeBalance -=
                        (transaction.Value + transaction.Fee);
                    _accountBalances[transaction.To].SafeBalance += transaction.Value;
                    transaction.IncludedInSafeBalance = true;
                }
            }
        }

        private void UpdateConfirmedAccountBalances(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                AddAccountBalancesIfMissing(transaction);

                _accountBalances[transaction.From].ConfirmedBalance -=
                    (transaction.Value + transaction.Fee);
                _accountBalances[transaction.To].ConfirmedBalance += transaction.Value;
            }
        }

        private void UpdatePendingAccountBalances(Transaction transaction)
        {
            AddAccountBalancesIfMissing(transaction);

            _accountBalances[transaction.From].PendingBalance -=
                (transaction.Value + transaction.Fee);
            _accountBalances[transaction.To].PendingBalance += transaction.Value;
        }

        private void RevertPendingAccountBalances(IEnumerable<Transaction> transactions)
        {
            foreach (Transaction transaction in transactions)
            {
                AddAccountBalancesIfMissing(transaction);

                _accountBalances[transaction.From].PendingBalance +=
                    (transaction.Value + transaction.Fee);
                _accountBalances[transaction.To].PendingBalance -= transaction.Value;
            }
        }

        private void AddAccountBalancesIfMissing(Transaction transaction)
        {
            if (!_accountBalances.ContainsKey(transaction.From))
            {
                _accountBalances.TryAdd(transaction.From, new AccountBalances());
            }
            if (!_accountBalances.ContainsKey(transaction.To))
            {
                _accountBalances.TryAdd(transaction.To, new AccountBalances());
            }
        }

        private bool CheckTransactionExists(string transactionDataHash)
        {
            if (_pendingTransactions.ContainsKey(transactionDataHash) ||
                _confirmedTransactions.ContainsKey(transactionDataHash))
            {
                return true;
            }

            return false;
        }

        private long GetAccountBalance(string address)
        {
            if (_accountBalances.ContainsKey(address))
            {
                return _accountBalances[address].ConfirmedBalance;
            }

            return 0;
        }

        private static void UpdateCandidateBlockData(
            Block candidateBlock,
            MiningResult miningResult)
        {
            candidateBlock.MinerProvidedHash = miningResult.BlockHash;
            candidateBlock.DateCreated = miningResult.DateCreated;
            candidateBlock.Nonce = miningResult.Nonce;
        }

        private static List<Transaction> GetMatchingTransactions(
            IEnumerable<Transaction> transactions,
            string address,
            bool confirmed)
        {
            List<Transaction> matchingTransactions = new List<Transaction>();
            foreach (Transaction transaction in transactions)
            {
                if (transaction.From == address ||
                    transaction.To == address)
                {
                    transaction.Confirmed = confirmed;
                    matchingTransactions.Add(transaction);
                }
            }

            return matchingTransactions;
        }
    }
}
