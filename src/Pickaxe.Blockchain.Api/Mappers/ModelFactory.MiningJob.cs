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
            TransactionDataBase[] transactions = candidateBlock
                .Transactions
                .Select(t => new TransactionDataBase(t))
                .ToArray();

            string json = JsonUtils.Serialize(transactions, false);
            byte[] hash = HashUtils.ComputeSha256(Utils.GetBytes(json));

            return new MiningJob
            {
                BlockIndex = candidateBlock.Index,
                TransactionsHash = hash.ToHex(),
                PreviousBlockHash = candidateBlock.PreviousBlockHash.ToHex(),
                Difficulty = candidateBlock.Difficulty
            };
        }
    }
}
