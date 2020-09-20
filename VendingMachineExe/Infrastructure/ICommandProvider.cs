using CommandLine;
using System.Collections.Generic;

namespace VendingMachine.CLI.Infrastructure
{
    public interface ICommandProvider
    {
        IEnumerable<Error> ParseErrors { get; set; }

        string GetProduct();

        int[] GetCoins();
    }
}
