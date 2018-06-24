using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common.Extensions;
using Pickaxe.Blockchain.Domain.Models;
using System;
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

        public static MinedBlock ToMiningResult(this BlockContract block)
        {
            return new MinedBlock
            {
                MinerAddress = block.MinedBy,
                BlockDataHash = block.BlockDataHash,
                DateCreated = DateTime.Parse(block.DateCreated).ToUniversalTime(),
                Nonce = block.Nonce,
                BlockHash = block.BlockHash
            };
        }
    }
}
