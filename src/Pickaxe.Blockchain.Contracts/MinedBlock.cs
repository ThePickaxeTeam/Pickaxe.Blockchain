using System.ComponentModel.DataAnnotations;

namespace Pickaxe.Blockchain.Contracts
{
    public class MinedBlock
    {
        [Required]
        public string BlockDataHash { get; set; }

        [Required]
        public string DateCreated { get; set; }

        [Required]
        public string Nonce { get; set; }

        [Required, MinLength(64), MaxLength(64)]
        public string BlockHash { get; set; }
    }
}
