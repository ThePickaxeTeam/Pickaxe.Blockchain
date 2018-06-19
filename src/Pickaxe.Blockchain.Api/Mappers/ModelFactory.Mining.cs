using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain.Models;
using System;
using System.Linq;

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

        public static MiningResult ToMiningResult(
            this MiningJobResult jobResult,
            string minerAddress)
        {
            return new MiningResult
            {
                MinerAddress = minerAddress,
                BlockDataHash = jobResult.BlockDataHash,
                DateCreated = DateTime.Parse(jobResult.DateCreated).ToUniversalTime(),
                Nonce = UInt64.Parse(jobResult.Nonce),
                BlockHash = jobResult.BlockHash
            };
        }
    }
}
