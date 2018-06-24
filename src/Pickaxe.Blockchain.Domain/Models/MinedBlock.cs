using System;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class MinedBlock
    {
        public string MinerAddress { get; set; }

        public string BlockDataHash { get; set; }

        public DateTime DateCreated { get; set; }

        public ulong Nonce { get; set; }

        public string BlockHash { get; set; }
    }
}
