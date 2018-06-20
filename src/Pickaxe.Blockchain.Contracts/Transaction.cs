namespace Pickaxe.Blockchain.Contracts
{
    public class Transaction
    {
        public string From { get; set; }

        public string To { get; set; }

        public long Value { get; set; }

        public long Fee { get; set; }

        public string DateCreated { get; set; }

        public string Data { get; set; }

        public string SenderPubKey { get; set; }

        public string TransactionDataHash { get; set; }

        public string[] SenderSignature { get; set; }

        public int? MinedInBlockIndex { get; set; }

        public bool? TransferSuccessful { get; set; }
    }
}
