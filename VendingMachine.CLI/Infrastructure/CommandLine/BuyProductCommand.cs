using CommandLine;

namespace VendingMachine.CLI.Infrastructure
{
    [Verb("buy")]
    public class BuyProductCommand
    {
        [Option("product")]
        public string Product { get; set; }

        [Option("coins")]
        public string Coins { get; set; }
    }
}