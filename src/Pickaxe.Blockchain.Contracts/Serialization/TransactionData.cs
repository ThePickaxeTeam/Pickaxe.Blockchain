namespace Pickaxe.Blockchain.Contracts.Serialization
{
    public class TransactionData
    {
        public string From { get; set; }

        public string To { get; set; }

        public long Value { get; set; }

        public long Fee { get; set; }

        public string DateCreated { get; set; }

        public string Data { get; set; }

        public string SenderPubKey { get; set; }
    }
}
