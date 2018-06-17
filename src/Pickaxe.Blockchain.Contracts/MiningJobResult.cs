namespace Pickaxe.Blockchain.Contracts
{
    public class MiningJobResult
    {
        public string Nonce { get; set; }

        public string TimestampTicks { get; set; }

        public string BlockHash { get; set; }
    }
}
