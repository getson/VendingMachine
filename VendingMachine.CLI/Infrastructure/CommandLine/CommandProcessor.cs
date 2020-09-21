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
        private const string _confirmPurchaseMessage = "Do you want to confirm the purchase [Y/N]?";
        private const string _insertRemainingAmountMessage = "Do you want to insert the remaining amount [Y/N]?";

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

            WriteHeaderOrFooter("Hello!");

            while (true)
            {
                try
                {
                    WriteSection("Products");

                    await WriteProducts();

                    await PurchaseOrder();

                    if (!_prompt.ReadBool(_newPurchaseMessage, true))
                    {
                        WriteHeaderOrFooter("Bye!", false);
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
            WriteSection("Purchase");
            while (true)
            {
                try
                {
                    var product = _command.GetProduct();

                    await _mediator.Send(new SelectProduct(product));

                    while (true)
                    {
                        try
                        {
                            var coins = _command.GetCoins();

                            await _mediator.Send(new InsertCoins(coins));
                            break;
                        }
                        catch (NotSufficientAmountException ex)
                        {
                            _terminal.WriteLine($"{ex.Message}");
                            _terminal.WriteLine($"Remaining Amount: {ex.RemainingAmount}");

                            if (!_prompt.ReadBool(_insertRemainingAmountMessage, true))
                            {
                                await CancelOrder();
                                return;
                            }
                        }
                        catch (VendingMachineValiationException ex)
                        {
                            _terminal.WriteLine($"{ex.Message}");
                        }
                    }

                    if (!_prompt.ReadBool(_confirmPurchaseMessage, true))
                    {
                        await CancelOrder();
                        break;
                    }

                    await _mediator.Send(new ProcessOrder(false));
                    break;
                }
                catch (NotSufficientChangeException ex)
                {
                    _terminal.WriteLine(ex.Message);
                    if (_prompt.ReadBool(_confirmPurchaseMessage, false))
                    {
                        await CancelOrder();
                        break;
                    }
                    await _mediator.Send(new ProcessOrder(true));
                    break;
                }
                catch (ProductNotAvailableException ex)
                {
                    _terminal.WriteLine(ex.Message);
                }
                catch (VendingMachineValiationException ex)
                {
                    _terminal.WriteLine($"{ex.Message}");
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

        private void WriteHeaderOrFooter(string message, bool isHeader = true)
        {
            if(!isHeader)
                _terminal.WriteLine();
            if (message?.Length < 73)
            {
                message = message.Length % 2 == 0 ? $" {message} " : $" {message} *";
                while (message.Length <= 76) message = $"*{message}*";
            }
            _terminal.WriteLine(message);
            if (isHeader)
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