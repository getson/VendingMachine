using CommandLine;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Commands;
using VendingMachine.Core.Domain;
using VendingMachine.Queries;

namespace VendingMachine.CLI.Infrastructure
{
    public class CommandProcessor
    {
        private readonly IMediator _mediator;
        private readonly ITerminal _terminal;
        private readonly ICommandPrompt _prompt;
        private readonly ICommandProvider _command;

        private const string _newPurchaseMessage = "Do you want to purchase another item [y/n]?";
        private const string _cancelPurchaseMessage = "Do you want to cancel the purchase [y/n]?";
        private const string _cancelTransactionMessage = "Do you want to cancel the transaction [y/n]?";
        private const string _new = "new";
        private const string _cancel = "cancel";

        public CommandProcessor(IMediator mediator, ITerminal terminal, ICommandPrompt prompt, ICommandProvider command)
        {
            _mediator = mediator;
            _terminal = terminal;
            _prompt = prompt;
            _command = command;
        }

        public void Execute()
        {
            var parseErrors = _command.ParseErrors;

            if ((parseErrors != null ? (parseErrors.Any() ? 1 : 0) : 0) != 0)
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

                    var products = _mediator.Send(new GetProducts())
                        .GetAwaiter()
                        .GetResult();

                    WriteProducts(products);

                    PurchaseOrder();

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

            void PurchaseOrder()
            {
                while (true)
                {
                    try
                    {
                        WriteSection("Purchase");

                        var product = _command.GetProduct();

                        _mediator.Send(new SelectProduct(product))
                            .GetAwaiter()
                            .GetResult();

                        var result = _mediator
                            .Send(new GetSelectedProductPrice())
                            .GetAwaiter()
                            .GetResult();

                        _terminal.WriteLine($"The price for '{product}' is: {result.Amount}!");

                        if (_prompt.ReadBool(_cancel, _cancelPurchaseMessage, false))
                        {
                            CancelOrder();
                            break;
                        }

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
                                
                                _mediator.Send(new InsertCoins(coins))
                                    .GetAwaiter()
                                    .GetResult();
                                break;
                            }
                            catch (NotFullyPaidException ex)
                            {
                                _terminal.WriteLine($"{ex.Message}: {ex.RemainingAmount}");

                                if (_prompt.ReadBool(_cancel, _cancelTransactionMessage, false))
                                {
                                    CancelOrder();
                                    return;
                                }
                                firstTransaction = false;
                            }
                        }

                        if (_prompt.ReadBool(_cancel, _cancelPurchaseMessage, false))
                        {
                            CancelOrder();
                            break;
                        }

                        _mediator.Send(new ProcessOrder())
                            .GetAwaiter()
                            .GetResult();

                        _terminal.WriteLine("The purchase completed!");
                        _terminal.WriteLine("Thank you!");

                        break;
                    }
                    catch (Exception ex)
                    {
                        _terminal.WriteError(ex.Message);
                    }
                }
            }

            void CancelOrder()
            {
                _terminal.WriteLine("The purchase canceled!");
                _mediator.Send(new CancelOrder())
                    .GetAwaiter()
                    .GetResult();
            }
        }

        private void WriteSection(string message)
        {
            _terminal.WriteLine();
            _terminal.WriteLine(">> " + message + ":");
            _terminal.WriteLine();
        }

        private void WriteProducts(IEnumerable<GetProductItemResult> products)
        {
            var maxProductNameWidth = 0;

            foreach (var product in products)
                maxProductNameWidth = maxProductNameWidth < product.ProductName.Length ? 
                    product.ProductName.Length : maxProductNameWidth;

            maxProductNameWidth += 2;

            foreach (var product in products)
                _terminal.WriteLine($" * {product.ProductName.PadRight(maxProductNameWidth)}: {product.Price}");
        }
    }
}