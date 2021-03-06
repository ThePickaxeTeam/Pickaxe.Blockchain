﻿using Pickaxe.Blockchain.Contracts.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Pickaxe.Blockchain.Contracts
{
    public class CreateTransactionRequest
    {
        [Required, MinLength(40), MaxLength(40)]
        public string From { get; set; }

        [Required, MinLength(40), MaxLength(40)]
        public string To { get; set; }

        [Required, Range(0, int.MaxValue)]
        public long Value { get; set; }

        [Required, Range(0, int.MaxValue)]
        public long Fee { get; set; }

        [Required]
        public string DateCreated { get; set; }

        public string Data { get; set; }

        [Required, MinLength(65), MaxLength(65)]
        public string SenderPubKey { get; set; }

        [Required]
        public string[] SenderSignature { get; set; }

        public static CreateTransactionRequest FromTransactionData(TransactionData transaction)
        {
            return new CreateTransactionRequest
            {
                From = transaction.From,
                To = transaction.To,
                Value = transaction.Value,
                Fee = transaction.Fee,
                DateCreated = transaction.DateCreated,
                Data = transaction.Data,
                SenderPubKey = transaction.SenderPubKey
            };
        }
    }
}
