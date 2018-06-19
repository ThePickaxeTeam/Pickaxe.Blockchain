using Nethereum.Hex.HexConvertors.Extensions;
using Pickaxe.Blockchain.Common;
using Pickaxe.Blockchain.Contracts;
using Pickaxe.Blockchain.Domain.Models;
using Pickaxe.Blockchain.Domain.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
