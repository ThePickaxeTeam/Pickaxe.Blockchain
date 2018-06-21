using System;

namespace Pickaxe.Blockchain.Domain
{
    public class NodeSettings : INodeSettings
    {
        public string About { get; private set; } = "Pickaxe.Blockchain v1.0.0";

        public string NodeId { get; private set; } = Guid.NewGuid().ToString();

        public string NodeUrl { get; private set; } = "http://localhost:64149/";

        public int CurrentDifficulty { get; private set; } = 4;

        public long MinFee { get; private set; } = 10;
    }
}
