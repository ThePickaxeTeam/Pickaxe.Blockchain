using System;
using System.Collections.Generic;
using System.Text;

namespace Pickaxe.Blockchain.Domain.Serialization
{
    internal class TransactionData
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Value { get; set; }

        public decimal Fee { get; set; }

        public string DateCreated { get; set; }

        public string Data { get; set; }

        public string SenderPublicKey { get; set; }

        public string DataHash { get; set; }

        public string[] SenderSignature { get; set; }

        public int MinedInBlockIndex { get; set; }

        public bool TransferSuccessful { get; set; }
    }
}
