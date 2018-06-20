using System.Collections.Generic;

namespace Pickaxe.Blockchain.Contracts
{
    public class Chain
    {
        public IList<Block> Blocks { get; set; }
    }
}
