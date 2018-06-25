using Pickaxe.Blockchain.Domain.Models;
using System;
using System.Linq;
using BlockContract = Pickaxe.Blockchain.Contracts.Block;

namespace Pickaxe.Blockchain.Domain.Mappers
{
    public static partial class ModelFactory
    {
        public static Block ToDomainModel(this BlockContract contract)
        {
            return new Block
            {
                Index = contract.Index,
                Transactions = contract.Transactions.Select(t => t.ToDomainModel()).ToList(),
                Difficulty = contract.Difficulty,
                PreviousBlockHash = contract.PreviousBlockHash,
                MinedBy = contract.MinedBy,
                Nonce = contract.Nonce,
                DateCreated = DateTime.Parse(contract.DateCreated).ToUniversalTime(),
                MinerProvidedHash = contract.BlockHash
            };
        }
    }
}
