using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Core.Domain.Services
{
    public interface IChangeCalculator
    {
        /// <summary>
        /// Calculate the minimum of coins for the required change
        /// </summary>
        /// <param name="coins"></param>
        /// <param name="change"></param>
        /// <returns></returns>
        IList<CoinWithQuantity> CalculateMinimum(IList<CoinWithQuantity> coins, int change);
    }
}
