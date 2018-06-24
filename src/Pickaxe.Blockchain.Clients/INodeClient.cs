using Pickaxe.Blockchain.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Clients
{
    public interface INodeClient
    {
        Task<Response<MiningJob>> GetMiningJob(string minerAddress);

        Task<Response<Block>> SubmitMinedBlock(
            MinedBlock miningJobResult,
            string minerAddress);

        Task<Response<Transaction>> CreateTransaction(CreateTransactionRequest request);

        Task<Response<EmptyPayload>> NotifyNewBlock(NewBlockNotification notification);

        Task<Response<List<Block>>> GetAllBlocks();
    }
}