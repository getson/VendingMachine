using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingMachine.CLI.Infrastructure
{
    public sealed class CommandProvider : ICommandProvider
    {
        private readonly IEnumerable<string> _verbCommands = new string[1] { "buy" };

        private readonly IEnumerable<Type> _verbTypes = new Type[1] {
            typeof(BuyProductCommand)
        };

        private readonly ICommandPrompt _commandPrompt;

        private BuyProductCommand BuyProductCommand { get; set; }

        public IEnumerable<Error> ParseErrors { get; set; }

        public CommandProvider(ICommandPrompt commandPrompt, string[] args)
        {
            _commandPrompt = commandPrompt;

            ParseArguments(args);
        }

        public string GetProduct()
        {
            if (!string.IsNullOrWhiteSpace(BuyProductCommand.Product))
            {
                return BuyProductCommand.Product;
            }

            return _commandPrompt.ReadValue("product", "Product Name", string.Empty,
                new Func<string, bool>(CommandValidators.NonEmptyValidator)
            );
        }

        public int[] GetCoins()
        {
            if (BuyProductCommand.Coins?.Any() ?? false)
            {
                return BuyProductCommand.Coins.Split(new[] { ' ' })
                    .Select(c => int.Parse(c))
                    .ToArray();
            }

            var coinsInput = _commandPrompt.ReadValue(
                "coins",
                "Coins (Insert coins space separated by! Ex: 10 20 50 100)",
                string.Empty,
                new Func<string, bool>(CommandValidators.NonEmptyValidator)
            );

            return coinsInput.Split(new[] { ' ' }).Select(c => int.Parse(c)).ToArray();
        }

        private void ParseArguments(string[] args)
        {
            ParseArguments(args, false);

            if (ParseErrors == null)
            {
                return;
            }

            ParseArguments(args, true);
        }

        private void ParseArguments(string[] args, bool ignoreErrors)
        {
            using Parser parser = new Parser(config =>
            {
                config.AutoHelp = false;
                config.AutoVersion = false;
                config.CaseSensitive = false;
                config.IgnoreUnknownArguments = ignoreErrors;
            });

            args = AddDefaultVerbIfNecessary(args);

            parser.ParseArguments(args, _verbTypes.ToArray())
                .WithParsed<BuyProductCommand>(x => BuyProductCommand = x)
                .WithNotParsed(errors => ParseErrors = errors);
        }

        private string[] AddDefaultVerbIfNecessary(string[] args)
        {
            if (args.Length == 0)
            {
                return new string[1] { "buy" };
            }

            if (_verbCommands.Any(str => str.Contains(args[0])) || !args[0].StartsWith("--"))
            {
                return args;
            }

            string[] commandArgs = new string[args.Length + 1];
            commandArgs[0] = "buy";

            Array.Copy(args, 0, commandArgs, 1, args.Length);

            return commandArgs;
        }
    }
}