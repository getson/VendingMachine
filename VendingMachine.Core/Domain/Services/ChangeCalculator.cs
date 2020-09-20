using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Core.Domain.Services
{
    public class ChangeCalculator : IChangeCalculator
    {
        private IList<CoinWithQuantity> Calculate(IList<CoinWithQuantity> coins, int change, int start = 0)
        {
            for (int i = start; i < coins.Count; i++)
            {
                CoinWithQuantity coin = coins[i];
                // no point calculating anything if no coins exist or the 
                // current denomination is too high
                if (coin.Quantity > 0 && coin.Denomination <= change)
                {
                    int remainder = change % coin.Denomination;
                    if (remainder < change)
                    {
                        int howMany = Math.Min(coin.Quantity,
                            (change - remainder) / coin.Denomination);

                        List<CoinWithQuantity> matches = new List<CoinWithQuantity>
                        {
                            new CoinWithQuantity((Coin)coin.Denomination, howMany)
                        };

                        int amount = howMany * coin.Denomination;
                        int changeLeft = change - amount;
                        if (changeLeft == 0)
                        {
                            return matches;
                        }

                        IList<CoinWithQuantity> subCalc = Calculate(coins, changeLeft, i + 1);
                        if (subCalc != null)
                        {
                            matches.AddRange(subCalc);
                            return matches;
                        }
                    }
                }
            }
            return null;
        }
        public IList<CoinWithQuantity> CalculateMinimum(IList<CoinWithQuantity> coins, int change)
        {
            // used to store the minimum matches
            IList<CoinWithQuantity> minimalMatch = null;
            int minimalCount = -1;

            IList<CoinWithQuantity> subset = coins;
            for (int i = 0; i < coins.Count; i++)
            {
                IList<CoinWithQuantity> matches = Calculate(subset, change);
                if (matches != null)
                {
                    int matchCount = matches.Sum(c => c.Quantity);
                    if (minimalMatch == null || matchCount < minimalCount)
                    {
                        minimalMatch = matches;
                        minimalCount = matchCount;
                    }
                }
                // reduce the list of possible coins
                subset = subset.Skip(1).ToList();
            }

            return minimalMatch;
        }
    }
}
