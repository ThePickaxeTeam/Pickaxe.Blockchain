namespace Pickaxe.Blockchain.Contracts
{
    public class MiningJobResult
    {
        public string BlockDataHash { get; set; }

        public string DateCreated { get; set; }

        public string Nonce { get; set; }

        public string BlockHash { get; set; }
    }
}
