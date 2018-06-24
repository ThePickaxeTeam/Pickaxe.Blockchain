using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Domain.Models;
using System.Linq;
using BlockContract = Pickaxe.Blockchain.Contracts.Block;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static BlockContract ToContract(this Block block)
        {
            return new BlockContract
            {
                Index = block.Index,
                Transactions = block.Transactions.Select(t => t.ToContract()).ToList(),
                Difficulty = block.Difficulty,
                PreviousBlockHash = block.IsGenesis ? null : block.PreviousBlockHash,
                MinedBy = block.MinedBy,
                BlockDataHash = block.DataHash.ToHex(),
                Nonce = block.Nonce,
                DateCreated = block.DateCreated.Iso8601Formatted(),
                BlockHash = block.MinerProvidedHash
            };
        }
    }
}
