namespace Pickaxe.Blockchain.Domain
{
    public interface INodeSettings
    {
        string About { get; }

        string NodeId { get; }

        string NodeUrl { get; }

        int CurrentDifficulty { get; }

        long MinFee { get; }
    }
}