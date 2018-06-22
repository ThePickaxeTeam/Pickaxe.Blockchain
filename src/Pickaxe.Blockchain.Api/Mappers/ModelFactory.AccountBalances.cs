using Pickaxe.Blockchain.Domain.Models;
using AccountBalancesContract = Pickaxe.Blockchain.Contracts.AccountBalances;

namespace Pickaxe.Blockchain.Api.Mappers
{
    public static partial class ModelFactory
    {
        public static AccountBalancesContract ToContract(this AccountBalances balances)
        {
            return new AccountBalancesContract
            {
                SafeBalance = balances.SafeBalance,
                ConfirmedBalance = balances.ConfirmedBalance,
                PendingBalance = balances.PendingBalance
            };
        }
    }
}
