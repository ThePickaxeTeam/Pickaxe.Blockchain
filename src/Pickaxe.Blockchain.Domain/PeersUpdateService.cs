using Pickaxe.Blockchain.Clients;
using Pickaxe.Blockchain.Domain.Models;
using System.Collections.Concurrent;

namespace Pickaxe.Blockchain.Domain
{
    public class PeersUpdateService : IPeersUpdateService
    {
        /// <summary>
        /// key - peer node Id, unique string
        /// value - peer node base URL (e.g. http://[host]:[port])
        /// </summary>
        private ConcurrentDictionary<string, string> _peers;

        public PeersUpdateService()
        {
            _peers = new ConcurrentDictionary<string, string>();
        }

        public void UpdatePeers(Block block)
        {
            foreach (string baseUrl in _peers.Values)
            {
                NodeClient nodeClient = new NodeClient(baseUrl);
                //nodeClient.Update(block.ToContract());
            }
        }
    }
}
