using CommandLine;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VendingMachine.Commands;
using VendingMachine.Core;
using VendingMachine.Core.Domain;
using VendingMachine.Queries;

namespace VendingMachine.CLI.Infrastructure
{
    public class CommandProcessor
    {
        private readonly IMediator _mediator;
        private readonly ITerminal _terminal;
        private readonly ICommandPrompt _prompt;
        private readonly ICommandParser _command;

        private const string _newPurchaseMessage = "Do you want to purchase another item [Y/N]?";
        private const string _cancelPurchaseMessage = "Do you want to cancel the purchase [Y/N]?";
        private const string _cancelTransactionMessage = "Do you want to cancel the transaction [Y/N]?";
        private const string _new = "new";
        private const string _cancel = "cancel";

        public CommandProcessor(
            IMediator mediator,
            ITerminal terminal,
            ICommandPrompt prompt,
            ICommandParser command
         )
        {
            _mediator = mediator;
            _terminal = terminal;
            _prompt = prompt;
            _command = command;
        }

        public async Task Execute()
        {
            var parseErrors = _command.ParseErrors;

            if (parseErrors?.Any() ?? false)
            {
                var stringList = new List<string>();
                var enumerator = _command.ParseErrors.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        var current = enumerator.Current;
                        if (current is TokenError tokenError)
                        {
                            stringList.Add(tokenError.Token);
                        }
                        else
                        {
                            _terminal.WriteError($"An error occurred: {current.Tag}");
                        }
                    }
                }
                finally
                {
                    if (enumerator != null)
                    {
                        enumerator.Dispose();
                    }
                }
                _terminal.WriteError($"Unrecognized command-line input arguments: '{string.Join(", ", stringList)}'.");
                return;
            }

            while (true)
            {
                try
                {
                    WriteSection("Products");

                    await WriteProducts();

                    await PurchaseOrder();

                    if (!_prompt.ReadBool(_new, _newPurchaseMessage, true))
                    {
                        _terminal.WriteLine("Bye!");
                        break;
                    }
                }
                catch (Exception ex)
                {
                    _terminal.WriteError(ex.Message);
                }
            }
        }

        private async Task CancelOrder()
        {
            _terminal.WriteLine("The purchase canceled!");
            await _mediator.Send(new CancelOrder());
        }

        private async Task PurchaseOrder()
        {
            while (true)
            {
                try
                {
                    WriteSection("Purchase");

                    var product = _command.GetProduct();

                    await _mediator.Send(new SelectProduct(product));

                    var coins = _command.GetCoins();
                    var firstTransaction = true;

                    while (true)
                    {
                        try
                        {
                            if (!firstTransaction)
                            {
                                var remaining = _command.GetCoins();
                                coins = coins.Concat(remaining).ToArray();
                            }

                            await _mediator.Send(new InsertCoins(coins));
                            break;
                        }
                        catch (NotFullyPaidException ex)
                        {
                            _terminal.WriteLine($"{ex.Message}: {ex.RemainingAmount}");

                            if (_prompt.ReadBool(_cancel, _cancelTransactionMessage, false))
                            {
                                await CancelOrder();
                                return;
                            }
                            firstTransaction = false;
                        }
                    }

                    if (_prompt.ReadBool(_cancel, _cancelPurchaseMessage, false))
                    {
                        await CancelOrder();
                        break;
                    }

                    await _mediator.Send(new ProcessOrder(false));
                    break;
                }
                catch (Exception ex)
                {
                    _terminal.WriteError(ex.Message);
                }
            }
        }

        private void WriteSection(string message)
        {
            _terminal.WriteLine();
            _terminal.WriteLine(">> " + message + ":");
            _terminal.WriteLine();
        }

        private async Task WriteProducts()
        {
            var products = await _mediator.Send(new GetProducts());

            var maxProductNameWidth = 0;

            foreach (var product in products)
            {
                maxProductNameWidth = maxProductNameWidth < product.ProductName.Length ?
                    product.ProductName.Length : maxProductNameWidth;
            }

            maxProductNameWidth += 2;

            foreach (var product in products)
            {
                _terminal.WriteLine($" * {product.ProductName.PadRight(maxProductNameWidth)}: {product.Price}");
            }
        }
    }
}