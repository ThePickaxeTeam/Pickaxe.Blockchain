using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain.Models;
using System;
using System.Linq;
using Block = Pickaxe.Blockchain.Domain.Models.Block;
using MinedBlockContract = Pickaxe.Blockchain.Contracts.MinedBlock;
using MinedBlock = Pickaxe.Blockchain.Domain.Models.MinedBlock;

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
            this MinedBlockContract jobResult,
            string minerAddress)
        {
            return new MinedBlock
            {
                MinerAddress = minerAddress,
                BlockDataHash = jobResult.BlockDataHash,
                DateCreated = DateTime.Parse(jobResult.DateCreated).ToUniversalTime(),
                Nonce = ulong.Parse(jobResult.Nonce),
                BlockHash = jobResult.BlockHash
            };
        }
    }
}
