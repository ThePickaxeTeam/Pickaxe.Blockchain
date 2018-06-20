using Pickaxe.Blockchain.Domain.Models;
using DebugInfoContract = Pickaxe.Blockchain.Contracts.DebugInfo;
using Chain = Pickaxe.Blockchain.Contracts.Chain;
using System.Linq;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static DebugInfoContract ToContract(this DebugInfo debugInfo)
        {
            return new DebugInfoContract
            {
                SelfUrl = debugInfo.SelfUrl,
                Chain = new Chain
                {
                    Blocks = debugInfo.Blocks.Select(b => b.ToContract()).ToList()
                }
            };
        }
    }
}
