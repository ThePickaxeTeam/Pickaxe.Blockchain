using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain.Mappers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Block = Pickaxe.Blockchain.Domain.Models.Block;
using BlockContract = Pickaxe.Blockchain.Contracts.Block;
using Transaction = Pickaxe.Blockchain.Domain.Models.Transaction;
using TransactionContract = Pickaxe.Blockchain.Contracts.Transaction;

namespace Pickaxe.Blockchain.Domain
{
    public class PeerSynchronizationService : IPeerSynchronizationService
    {
        /// <summary>
        /// key - peer node Id, unique string
        /// value - peer node base URL (e.g. http://[host]:[port])
        /// </summary>
        private ConcurrentDictionary<string, string> _peers;
        private ConcurrentDictionary<string, INodeClient> _peerNodeClients;

        public PeerSynchronizationService()
        {
            _peers = new ConcurrentDictionary<string, string>();
            _peerNodeClients = new ConcurrentDictionary<string, INodeClient>();
        }

        public Dictionary<string, string> GetPeers()
        {
            return _peers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public int GetPeersCount()
        {
            return _peers.Count;
        }

        public async Task BroadcastNewBlockNotification(
            int blocksCount,
            long cumulativeDifficulty,
            string nodeUrl)
        {
            NewBlockNotification notification = new NewBlockNotification
            {
                BlocksCount = blocksCount,
                CumulativeDifficulty = cumulativeDifficulty,
                NodeUrl = nodeUrl
            };

            foreach (string peerUrl in _peers.Values)
            {
                await SendNewBlockNotification(notification, peerUrl, 5).ConfigureAwait(false);
            }
        }

        public async Task BroadcastNewTransaction(Transaction transaction)
        {
            CreateTransactionRequest request = transaction.ToCreateRequest();

            foreach (string peerBaseUrl in _peers.Values)
            {
                await SendCreateTransactionRequest(request, peerBaseUrl, 5).ConfigureAwait(false);
            }
        }

        public async Task<List<Block>> GetAllBlocks(string peerUrl)
        {
            List<BlockContract> peerChain = await GetAllBlocks(peerUrl, 5).ConfigureAwait(false);
            return peerChain.Select(b => b.ToDomainModel()).ToList();
        }

        private async Task SendNewBlockNotification(
            NewBlockNotification notification,
            string peerUrl,
            int maxRetries)
        {
            if (!_peerNodeClients.ContainsKey(peerUrl))
            {
                return;
            }

            INodeClient nodeClient = _peerNodeClients[peerUrl];
            Response<EmptyPayload> response;
            int retries = 0;
            do
            {
                retries++;
                response = await nodeClient.NotifyNewBlock(notification).ConfigureAwait(false);
            } while (response.Status == Status.Failed && retries < maxRetries);
        }

        private async Task SendCreateTransactionRequest(
            CreateTransactionRequest request,
            string peerUrl,
            int maxRetries)
        {
            if (!_peerNodeClients.ContainsKey(peerUrl))
            {
                return;
            }

            INodeClient nodeClient = _peerNodeClients[peerUrl];
            Response<TransactionContract> response;
            int retries = 0;
            do
            {
                retries++;
                response = await nodeClient.CreateTransaction(request).ConfigureAwait(false);
            } while (response.Status == Status.Failed && retries < maxRetries);
        }

        private async Task<List<BlockContract>> GetAllBlocks(string peerUrl, int maxRetries)
        {
            if (!_peerNodeClients.ContainsKey(peerUrl))
            {
                return new List<BlockContract>();
            }

            INodeClient nodeClient = _peerNodeClients[peerUrl];
            Response<List<BlockContract>> response;
            int retries = 0;
            do
            {
                retries++;
                response = await nodeClient.GetAllBlocks().ConfigureAwait(false);
            } while (response.Status == Status.Failed && retries < maxRetries);

            if (response.Status == Status.Success)
            {
                return response.Payload;
            }
            else
            {
                return new List<BlockContract>();
            }
        }
    }
}
