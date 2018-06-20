namespace Pickaxe.Blockchain.Domain
{
    public interface INodeSettings
    {
        string About { get; }

        string NodeId { get; }

        string ChainId { get; }

        string NodeUrl { get; }

        int CurrentDifficulty { get; }
    }
}