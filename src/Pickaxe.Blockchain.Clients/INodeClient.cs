using Pickaxe.Blockchain.Contracts;
using System.Threading.Tasks;

namespace Pickaxe.Blockchain.Clients
{
    public interface INodeClient
    {
        Task<Response<MiningJob>> GetMiningJob(string minerAddress);

        Task<Response<EmptyPayload>> SubmitMiningJob(
            MiningJobResult result,
            string minerAddress);
    }
}