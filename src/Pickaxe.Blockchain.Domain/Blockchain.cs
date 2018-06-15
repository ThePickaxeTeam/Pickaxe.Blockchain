using System;
using System.Collections.Generic;

namespace Pickaxe.Blockchain.Domain
{
    public class Blockchain : IBlockchain
    {
        private IList<Block> _blocks;

        public Blockchain()
        {
            _blocks = new List<Block>();
        }
    }
}
