using System.ComponentModel;

namespace Pickaxe.Blockchain.Domain.Enums
{
    public enum BlockValidationResult
    {
        Ok = 0,

        [Description("Block not found or already mined.")]
        BlockNotFound = 1,

        [Description("Block hash difficulty mismatch.")]
        BlockHashDifficultyMismatch = 2,

        [Description("Invalid block hash.")]
        InvalidBlockHash = 3,

        [Description("Block has already been mined.")]
        BlockAlreadyMined = 4
    }
}
