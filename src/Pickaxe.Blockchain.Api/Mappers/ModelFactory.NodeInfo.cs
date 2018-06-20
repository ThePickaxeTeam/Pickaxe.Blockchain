using Pickaxe.Blockchain.Domain.Models;
using NodeInfoContract = Pickaxe.Blockchain.Contracts.NodeInfo;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static NodeInfoContract ToContract(this NodeInfo nodeInfo)
        {
            return new NodeInfoContract
            {
                About = nodeInfo.About,
                NodeId = nodeInfo.NodeId,
                ChainId = nodeInfo.ChainId,
                NodeUrl = nodeInfo.NodeUrl,
                Peers = nodeInfo.Peers,
                CurrentDifficulty = nodeInfo.CurrentDifficulty,
                BlocksCount = nodeInfo.BlocksCount,
                CumulativeDifficulty = nodeInfo.CumulativeDifficulty,
                ConfirmedTransactions = nodeInfo.ConfirmedTransactions,
                PendingTransactions = nodeInfo.PendingTransactions
            };
        }
    }
}
