using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.Core.Domain.Services
{
    public class ChangeCalculator : IChangeCalculator
    {
        private IList<Coin> Calculate(IList<Coin> coins, int change, int start = 0)
        {
            for (int i = start; i < coins.Count; i++)
            {
                Coin coin = coins[i];
                // no point calculating anything if no coins exist or the 
                // current denomination is too high
                if (coin.Count > 0 && coin.Denomination <= change)
                {
                    int remainder = change % coin.Denomination;
                    if (remainder < change)
                    {
                        int howMany = Math.Min(coin.Count,
                            (change - remainder) / coin.Denomination);

                        List<Coin> matches = new List<Coin>
                        {
                            new Coin(coin.Denomination, howMany)
                        };

                        int amount = howMany * coin.Denomination;
                        int changeLeft = change - amount;
                        if (changeLeft == 0)
                        {
                            return matches;
                        }

                        IList<Coin> subCalc = Calculate(coins, changeLeft, i + 1);
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
        public IList<Coin> CalculateMinimum(IList<Coin> coins, int change)
        {
            // used to store the minimum matches
            IList<Coin> minimalMatch = null;
            int minimalCount = -1;

            IList<Coin> subset = coins;
            for (int i = 0; i < coins.Count; i++)
            {
                IList<Coin> matches = Calculate(subset, change);
                if (matches != null)
                {
                    int matchCount = matches.Sum(c => c.Count);
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
