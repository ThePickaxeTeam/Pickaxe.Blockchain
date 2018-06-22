using System.ComponentModel;

namespace Pickaxe.Blockchain.Domain.Enums
{
    public enum CreateTransactionResult
    {
        Ok = 0,
        [Description("Signature verification failed.")]
        VerificationFailed = 1,
        [Description("Duplicate transaction.")]
        DuplicateTransaction = 2,
        [Description("Insufficient balance.")]
        InsufficientBalance = 3,
        [Description("Fee less than minimum fee.")]
        InsufficientFee = 4
    }
}
