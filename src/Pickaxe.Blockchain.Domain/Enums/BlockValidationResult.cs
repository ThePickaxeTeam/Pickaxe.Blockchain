using System.ComponentModel;

namespace Pickaxe.Blockchain.Domain.Enums
{
    public enum BlockValidationResult
    {
        Ok = 0,

        [Description("Miner address not found.")]
        MinerAddressNotFound = 1,

        [Description("Block data hash mismatch.")]
        BlockDataHashMismatch = 2,

        [Description("Block hash difficulty mismatch.")]
        BlockHashDifficultyMismatch = 3,

        [Description("Invalid block hash.")]
        InvalidBlockHash = 4,

        [Description("Block has already been added.")]
        BlockAlreadyAdded = 5
    }
}
