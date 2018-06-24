using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Contracts;
using System.Collections.Concurrent;
using System.Threading.Tasks;

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

        public async Task BroadcastNewTransaction(CreateTransactionRequest request)
        {
            foreach (string peerBaseUrl in _peers.Values)
            {
                await SendCreateTransactionRequest(request, peerBaseUrl, 5).ConfigureAwait(false);
            }
        }

        private async Task SendNewBlockNotification(
            NewBlockNotification notification,
            string peerUrl,
            int maxRetries)
        {
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
            INodeClient nodeClient = _peerNodeClients[peerUrl];
            Response<Transaction> response;
            int retries = 0;
            do
            {
                retries++;
                response = await nodeClient.CreateTransaction(request).ConfigureAwait(false);
            } while (response.Status == Status.Failed && retries < maxRetries);
        }
    }
}
