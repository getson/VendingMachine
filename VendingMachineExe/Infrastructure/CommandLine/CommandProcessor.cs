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

        private const string _cancelPurchaseMessage = "Do you want to cancel the purchase?";
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
                    WriteSection("Purchase");

                    var product = _command.GetProduct();

                    _mediator.Send(new SelectProduct(product));

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

                    while (true)
                    {
                        try
                        {
                            _mediator.Send(new InsertCoins(coins));
                            break;
                        }
                        catch (NotFullPaidException ex)
                        {
                            _terminal.WriteLine($"{ex.Message}: {ex.RemainingAmount}");

                            if (_prompt.ReadBool(_cancel, _cancelPurchaseMessage, false))
                            {
                                CancelOrder();
                                return;
                            }
                        }
                    }

                    if (_prompt.ReadBool(_cancel, _cancelPurchaseMessage, false))
                    {
                        CancelOrder();
                        break;
                    }

                    _mediator.Send(new ProcessOrder());

                    _terminal.WriteLine("The purchase completed!");
                    _terminal.WriteLine("Thank you!");

                    break;
                }
                catch (Exception ex)
                {
                    _terminal.WriteError(ex.Message);
                }
            }

            void CancelOrder()
            {
                _terminal.WriteLine("The purchase canceled!");
                _mediator.Send(new CancelOrder());
            }
        }

        private void WriteSection(string message)
        {
            _terminal.WriteLine();
            _terminal.WriteLine(">> " + message + ":");
            _terminal.WriteLine();
        }
    }
}