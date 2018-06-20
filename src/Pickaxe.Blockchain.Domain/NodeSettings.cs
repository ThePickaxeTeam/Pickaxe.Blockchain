namespace Pickaxe.Blockchain.Domain
{
    public class NodeSettings : INodeSettings
    {
        public int Difficulty { get; private set; } = 4;
    }
}
