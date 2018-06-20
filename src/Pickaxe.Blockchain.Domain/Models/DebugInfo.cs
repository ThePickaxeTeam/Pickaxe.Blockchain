using System.Collections.Generic;

namespace Pickaxe.Blockchain.Domain.Models
{
    public class DebugInfo
    {
        public string SelfUrl { get; set; }

        public IList<Block> Blocks { get; set; }
    }
}
