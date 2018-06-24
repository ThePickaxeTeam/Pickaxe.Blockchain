using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Contracts;
using System;
using System.Linq;
using Block = Pickaxe.Blockchain.Domain.Models.Block;
using MinedBlock = Pickaxe.Blockchain.Domain.Models.MinedBlock;
using MinedBlockContract = Pickaxe.Blockchain.Contracts.MinedBlock;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static MiningJob ToMiningJob(this Block candidateBlock)
        {
            return new MiningJob
            {
                BlockIndex = candidateBlock.Index,
                Difficulty = candidateBlock.Difficulty,
                ExpectedReward = candidateBlock.Transactions.Sum(t => t.Fee),
                RewardAddress = candidateBlock.MinedBy,
                TransactionsIncluded = candidateBlock.Transactions.Count,
                BlockDataHash = candidateBlock.DataHash.ToHex()
            };
        }

        public static MinedBlock ToDomainModel(
            this MinedBlockContract block,
            string minerAddress)
        {
            return new MinedBlock
            {
                MinerAddress = minerAddress,
                BlockDataHash = block.BlockDataHash,
                DateCreated = DateTime.Parse(block.DateCreated).ToUniversalTime(),
                Nonce = ulong.Parse(block.Nonce),
                BlockHash = block.BlockHash
            };
        }
    }
}
